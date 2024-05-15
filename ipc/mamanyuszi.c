#include <errno.h>
#include <fcntl.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/ipc.h>
#include <sys/msg.h>
#include <sys/stat.h>
#include <sys/types.h>
#include <time.h>
#include <unistd.h>
#include <wait.h>
#include <ctype.h>

#define MAX_HOSSZ 1024

#define locsol SIGRTMIN
#define keszenall SIGRTMIN + 1
#define valasztott SIGRTMIN + 2

#define pipenev "/tmp/xikvif"


int mqid;

struct msgstring {
  long type;
  char data[MAX_HOSSZ];
};

void list_menu_commands() {
  printf("\nXIKVIF - BEADANDO 2 - MENU\n1 - list poems\n2 - add new poem\n3 - "
         "edit existing poem\n4 - delete existing poem\n5 - send out child\n6 "
         "- quit\nChoose a command: \n");
}

int poem_amount() {
    int fd = open("versek.bin", O_RDONLY);
    if (fd == -1) {
        perror("Error opening file");
        return -1; 
    }

    char ch;
    ssize_t bytes_read;
    int newline_count = 0;

    while ((bytes_read = read(fd, &ch, 1)) > 0) {
        if (ch == '\n') {
            newline_count++;
        }
    }

    close(fd);

    if (bytes_read == -1) {
        perror("Error reading file");
        return -1;
    }

    return newline_count;
}

void list_all_poems() {
  

  if (poem_amount() == 0){
    printf("No poems to display.\n");
    return;
  }

  int fid = open("versek.bin", O_RDONLY);
  if (fid == -1) {
    perror("Failed to open file");
    return;
  }

  int index = 1;
  int was_newline = 0;
  ssize_t bytes_read;
  char buffer[MAX_HOSSZ];
  printf("\n-----\n");
  printf("%d: ", index++);

  while ((bytes_read = read(fid, buffer, sizeof(buffer))) > 0) {
    for (int i = 0; i < bytes_read; i++) {
      if (buffer[i] == '\n') {
        was_newline = 1;
      } else {
        if(was_newline){
          was_newline = 0;
          printf("\n%d: ", index++);
        }
        putchar(buffer[i]);
      }
    }
  }
  printf("\n-----\n");

  close(fid);
}

void add_poem() {
  // init
  int fid;
  char input[MAX_HOSSZ];

  // uj beolvasasa
  printf("Compose your poem:\n");

  while ((getchar()) != '\n'); // tiszta input

  fgets(input, sizeof(input), stdin);

  // megnyitni a filet
  fid = open("versek.bin", O_RDWR | O_CREAT | O_APPEND, 0644);
  if (fid == -1) {
    perror("Failed to open file");
    return;
  }

  // beiras
  ssize_t bytes_written = write(fid, input, strlen(input));
  if (bytes_written == -1) {
    perror("writing unsuccessful");
    close(fid);
    return;
  }

  printf("\nPoem added successfully.");
  close(fid);
}

int is_numeric(const char *str) {
    if (str == NULL || *str == '\0') {
        return 0; 
    }
    while (*str) {
        if (!isdigit(*str)) {
            return 0; //no
        }
        str++; 
    }
    return 1; //yes
}

int read_command_number(){
  char input[MAX_HOSSZ];
  if(scanf("%s",input) != 1){
    printf("Unsuccessful input. (scanf)\n");
    return -1;
  } 

  int converted;
  if(!is_numeric(input) || (converted = atoi(input)) <= 0){
    printf("Please enter a positive numeric value.\n");
    return -1;
  }

  return converted;
}

int validate(int command){
  
  if(poem_amount() == 0){
    printf(command == 0 ? "No poems to delete.\n" : "No poems to edit.\n");
    return -1;
  }
  printf(command == 0 ? "Enter the index of the poem you want to delete: " : "Enter the index of the poem you want to edit: ");

  int index = read_command_number();

  if (index > poem_amount()) {
    printf("Only %d poems in file.\n", poem_amount());
    printf("Index exceeds the number of poems in the file.\n");
    return -2;
  }

  return index;
}

void rename_file(){
  if (rename("temp.bin", "versek.bin") != 0) {
    perror("Failed to rename temporary file");
    return;
  }
}

void delete_poem_at_index(int index) {

  if(poem_amount() == 0){
    printf("No poems to delete.\n");
    return;
  }

  int fid = open("versek.bin", O_RDONLY);
  if (fid == -1) {
    perror("Unable to open file");
    return;
  }

  int temp_fid = open("temp.bin", O_WRONLY | O_CREAT | O_TRUNC, 0644);
  if (temp_fid == -1) {
    perror("Failed to open temporary file");
    close(fid);
    return;
  }

  int line = 1;
  char buffer;

  while (read(fid, &buffer, 1) > 0) {
    if (line == index) {
      while (buffer != '\n') {
        if (read(fid, &buffer, 1) == 0) {
          break;
        }
      }
      line++;
      continue;
    }

    write(temp_fid, &buffer, 1);

    if (buffer == '\n') {
      line++;
    }
  }

  close(fid);
  close(temp_fid);
  
  rename_file();

  printf("\nPoem at index %d deleted successfully.\n", index);
}

void delete_poem() {

  int index = validate(0);

  if(index >0){
    delete_poem_at_index(index);
  }
}

void edit_poem() {
  
  int index = validate(1);

  if(index < 0){
    return;//assigning index unsuccessful, az okat elmondja a validate ugyis
  }

  int fid = open("versek.bin", O_RDONLY);
  if (fid == -1) {
    perror("Unable to open file");
    return;
  }

  int temp_fid = open("temp.bin", O_WRONLY | O_CREAT | O_TRUNC, 0644);
  if (temp_fid == -1) {
    perror("Failed to open temporary file");
    close(fid);
    return;
  }

  
  int line = 1;
  char buffer;
  ssize_t bytes;
  off_t position;
  int found = 0;

  while ((bytes = read(fid, &buffer, 1)) > 0) { //bemasoljuk ameg elerunk az editelni kivanthoz
    if (line == index) {                        
      printf("Poem to edit: ");
      found = 1;
      while (buffer != '\n' && bytes > 0) { // kiirja kepernyore azt a sort aminel egyezik
        putchar(buffer);
        if ((bytes = read(fid, &buffer, 1)) == 0) {
          break;
        }
      }
      position = lseek(fid, 0, SEEK_CUR);//megjegyezzuk ezt a sort, ide kerul az uj
      break; 
    }

    write(temp_fid, &buffer, 1); // amugy csak masoljuk a regi tartalmat

    if (buffer == '\n') {
      line++;
    }
  }

  close(fid);
  close(temp_fid);

  if (found) {//ha talaltunk editelni kivant verset
    char input[MAX_HOSSZ];
    printf("\nCompose edited poem: "); // utana megker hogy irjak egy ujat
    while ((getchar()) != '\n'); // clear input
    fgets(input, sizeof(input), stdin);

    // ujra kinyitom oket
    fid = open("versek.bin", O_RDONLY);
    if (fid == -1) {
      perror("Unable to open file");
      return;
    }
    // oda ugrok ahol voltam utoljara
    lseek(fid, position, SEEK_SET);

    temp_fid = open("temp.bin", O_WRONLY | O_APPEND, 0644);
    if (temp_fid == -1) {
      perror("Failed to open temporary file");
      close(fid);
      return;
    }

    ssize_t bytes_written = write(temp_fid, input, strlen(input)); // beleirjuk az uj fileba
    if (bytes_written == -1) {
      perror("Writing unsuccessful");
      close(fid);
      close(temp_fid);
      return;
    }

    // folytatom az atmasolast a tempbe vegig
    while (read(fid, &buffer, 1) > 0) {
      write(temp_fid, &buffer, 1); 
    }
    close(fid);
    close(temp_fid);

    if (rename("temp.bin", "versek.bin") != 0) {
      perror("Failed to rename temporary file");
      return;
    }
    printf("Poem at index %d edited successfully.\n", index);
  } else {
    printf("Poem not found!\n");
  }
}

int find_index_of_poem(char *poem) {
  
  int fd = open("versek.bin", O_RDONLY);
  if (fd == -1) {
    perror("Error opening file");
    return -1; // Error opening file
  }

  char ch;
  char line[MAX_HOSSZ];
  int index = 1;
  int line_pos = 0;
  ssize_t bytes_read;

  while ((bytes_read = read(fd, &ch, 1)) > 0) {
    if (ch == '\n') {
      line[line_pos] = '\0';

      if (strcmp(line, poem) == 0) {
        close(fd);
        return index;
      }
      line_pos = 0;
      index++;
    } else {
      if (line_pos < MAX_HOSSZ - 1) {
        line[line_pos++] = ch;
      } else {
        while ((bytes_read = read(fd, &ch, 1)) > 0 && ch != '\n') { }//olvas ameg nincs vege mert mar tulcsordult
        index++;
        line_pos = 0;
      }
    }
  }

  close(fd);

  if (bytes_read == -1) {
    perror("Error reading file");
    return -1; 
  }

  return -1;
}

char *find_poem_at_index(int index) {

    int fd = open("versek.bin", O_RDONLY);
    if (fd == -1) {
        perror("Error opening file");
    }

    char ch;
    char line[MAX_HOSSZ];
    int current_index = 1;
    int line_pos = 0;
    ssize_t bytes_read;

    while ((bytes_read = read(fd, &ch, 1)) > 0) {
        if (ch == '\n') {
            line[line_pos] = '\0';

            if (current_index == index) {
                close(fd);
                return strdup(line);
            }

            current_index++;
            line_pos = 0;

        } else {
            if (line_pos < MAX_HOSSZ - 1) {
                line[line_pos++] = ch;
            } else {
                while ((bytes_read = read(fd, &ch, 1)) > 0 && ch != '\n') { }//tulcsordult, csak olvassunk tovabb ujsorig / a vegeig
                current_index++;
                line_pos = 0;
            }
        }
    }

    close(fd);

    if (bytes_read == -1) {
        perror("Error reading file");
        return NULL; 
    }

    return NULL;
}

int random_index(int i) { 
  return (rand() % i); //always from 0..i-1
}

void remove_element(pid_t *pidek, int *meret, pid_t X) {
  
  int uj = 0;

  for (int i = 0; i < *meret; i++) {
    if (pidek[i] != X) {
      pidek[uj++] = pidek[i];
    }
  }

  *meret = uj;
}

void kill_all(pid_t gyerekek[], size_t meret) {
  for (int i = 0; i < meret; i++) {
    kill(gyerekek[i], SIGTERM);
    wait(NULL);
  }
  printf("\nAll children successfully terminated.\n");
}

void locsolo_handler(int signum) {
  printf("PID: %d - A child goes to 'water' some girls...\n", getpid());
}

void valasztott_handler(int signum) {
  printf("\nA child has sent his chosen poem...\n");
  struct msgstring uzenet;
  
  ssize_t bytes_received = msgrcv(mqid, &uzenet, sizeof(uzenet.data), 1, 0);

  if (bytes_received == -1) {
      if (errno == ENOMSG) {
          printf("No message available in the queue.\n");
      } else {
          perror("msgrcv");
          return;
      }
  } else {
      printf("\nParent received message from child in msq:\n%s\n", uzenet.data);
  }

  //a gyerek halalat varjuk
  pid_t terminated = wait(NULL);
  printf("\nPID: %d - Child terminated normally.\n", terminated);
}

void szulo_handler(int signum) {
  printf("\nPID: %d - Parent hears that the child has arrived.\n", getpid());

  int first_poem_index = random_index(poem_amount()) + 1;//ez egy szam 0..max-1 => 1..max
  int second_poem_index = random_index(poem_amount()) + 1;

  while(first_poem_index == second_poem_index){
    second_poem_index = random_index(poem_amount()) + 1;
  }

  char *msg1 = find_poem_at_index(first_poem_index);
  char *msg2 = find_poem_at_index(second_poem_index);

  int pipefd;
  if((pipefd = open(pipenev, O_RDWR)) < 0){
    perror("pipe open error");
    return;
  }
  write(pipefd, msg1, strlen(msg1) + 1);
  write(pipefd, msg2, strlen(msg2) + 1);
  close(pipefd);

  free(msg1);
  free(msg2);
}

void terminate_handler(int signum) {
  printf("\nPID: %d - The child was killed.\n", getpid());
  exit(0);
}

void gyerek_locsol(sigset_t mask, char* nev,int sleepytime)
{
  
  sigdelset(&mask, locsol); // reagalni fog jelzesekre a szulotol
  sigdelset(&mask, SIGTERM);
  sigprocmask(SIG_BLOCK, &mask, NULL);

  char poem_buffer[2 * MAX_HOSSZ]; // vers buffer
  char *poem1;
  char *poem2;
  char buffer_char;
  int index = 0;

  // wait for permission: locsol
  sigsuspend(&mask);
  kill(getppid(), keszenall); // jelzi hogy ottvan

  int pipefd;
  if((pipefd = open(pipenev, O_RDONLY)) < 0){
    perror("pipe open error");
    return;
  }
  while (read(pipefd, &buffer_char, sizeof(buffer_char))) {
    poem_buffer[index++] = buffer_char;
  }
  close(pipefd);
  

  // terminator szerint szet szedi oket
  poem1 = poem_buffer;
  poem2 = poem_buffer + strlen(poem1) + 1;

  //kiir
  printf("\nPID: %d - %s child says:\n",getpid(), nev);
  printf("\nPoem 1: %s", poem1);
  printf("\nPoem 2: %s", poem2);
  printf("\nPID: %d - %s child chooses a poem.\n",getpid(), nev);
  
  //valasztas
  char *poems[2] = {poem1, poem2};
  int chosen_index = (random_index(100) > 50) ? 1 : 0;//igy jobban tetszik az 50-50%
  char* chosen_poem = poems[chosen_index];

  //torles
  int chosen_poem_index = find_index_of_poem(chosen_poem);
  delete_poem_at_index(chosen_poem_index);

  //szimulalunk egy kis idot amig locsol
  sleep(sleepytime);

  // viszakuldes
  struct msgstring uzenet;
  uzenet.type = 1;
  strcpy(uzenet.data, chosen_poem);

  if((msgsnd(mqid, &uzenet, sizeof(uzenet.data), 0)) < 0){
    perror("msgsnd error");
  }
  kill(getppid(), valasztott);

  //elmondas
  printf("\nPID: %d - %s child tells his poem:\n%s.\nSzabad-e locsolni!\n", getpid(), nev, chosen_poem);

  return;
}

int main(int argc, char *argv[]) {
  //kulcs
  key_t kulcs = ftok(argv[0], 921);

  //mq
  mqid = msgget(kulcs, IPC_CREAT | IPC_NOWAIT | 0666);
  if (mqid == -1) {
      perror("msgget");
      return -1;
  }

  // gyerek
  int children_available = 4;

  // randomizer
  srand(time(0));

  // pidek
  pid_t pid1, pid2, pid3, pid4;

  // pipe
  //unlink(pipenev);//ha mar letezik veletlenul
  if(mkfifo(pipenev, 0666) < 0){
    perror("mkfifo error");
    return -1;
  }
  int pipefd;

  // handling
  signal(locsol, locsolo_handler);
  signal(keszenall, szulo_handler);
  signal(valasztott, valasztott_handler);
  signal(SIGTERM, terminate_handler);

  // masking
  sigset_t mask;
  if(sigfillset(&mask) != 0){
    perror("sigfillset error");
    return -1;
  }
  

  pid1 = fork();
  if (pid1 > 0) { // szulo
    pid2 = fork();
    if (pid2 > 0) { // szulo
      pid3 = fork();
      if (pid3 > 0) { // szulo
        pid4 = fork();
        if (pid4 > 0) { // SZULO MAIN

          pid_t gyerekek[4] = {pid1, pid2, pid3, pid4}; // itt mar van ertekuk -.-
          int choice;
          struct msgstring uzenet;

          sigdelset(&mask, keszenall);
          sigdelset(&mask, valasztott);
          sigprocmask(SIG_BLOCK, &mask, NULL);

          do {
            list_menu_commands();

            if ((choice = read_command_number()) < 0){
              continue;
            }
            if (choice < 1 || choice > 6) {
              printf("Please enter a number between 1 and 6.\n");
              continue;
            }

            switch (choice) {
            case 1:
              list_all_poems();
              break;
            case 2:
              add_poem();
              break;
            case 3:
              edit_poem();
              break;
            case 4:
              delete_poem();
              break;
            case 5:

              if (children_available && poem_amount() >= 2) {
                
                // elkuldom locsolni
                pid_t kivalasztott = gyerekek[random_index(children_available)];
                kill(kivalasztott,locsol);
                remove_element(gyerekek,&children_available,kivalasztott);

              } else if (children_available == 0){
                
                printf("\nNo child available...\n");

              } else {
                
                printf("\nNot enough poems. Please compose new (2).\n");

              }

              break;
            case 6:
              
              printf("\nExiting the program.\n");

              // megolom a gyerekeket
              kill_all(gyerekek, children_available);

              // destroy pipes, msgqueues
              if(unlink(pipenev) < 0){
                perror("unlink error");
                return -1;
              }
              if(msgctl(mqid, IPC_RMID, NULL) < 0){
                perror("msgctl remove error");
                return -1;
              }

              break;
            default:
              printf("\nInvalid choice. Please enter a number between 1 and 6.\n");
            }
          } while (choice != 6);

        } else if (pid4 < 0){ 
            perror("forking 4"); 
            return -1; 
          } 
          else {// gyerek4
            gyerek_locsol(mask,"4th",random_index(10)+10);//minimum 10 masodperc egy locsolas
          }
      } else if (pid3 < 0){ 
            perror("forking 3"); 
            return -1;
          } 
        else {// gyerek3
          gyerek_locsol(mask,"3rd",random_index(10)+10);
        }
    } else if (pid2 < 0){ 
            perror("forking 2"); 
            return -1; 
          } 
        else {// gyerek2
          gyerek_locsol(mask,"2nd",random_index(10)+10);
        }
  } else if (pid1 < 0){ 
            perror("forking 1"); 
            return -1; 
          } 
        else {// gyerek1
          gyerek_locsol(mask,"1st",random_index(10)+10);
        }
  return 0;
}