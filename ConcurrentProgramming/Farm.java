package beadandov2;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Random;
import java.util.concurrent.locks.ReentrantReadWriteLock;

public class Farm {

    private int _hossz;
    private int _szelesseg;
    public Object[][] field;
    public boolean running;
    private List<Juh> juhok;
    private List<Kutya> kutyak;
    private final ReentrantReadWriteLock lock = new ReentrantReadWriteLock(true);//fair

    public Farm(int hossz, int szelesseg) {

        if ((hossz - 1) % 3 != 2 || (szelesseg - 1) % 3 != 2 || (hossz / 3 * szelesseg / 3 < 10)) {
            throw new IllegalArgumentException("3 tobbszorosenel 2vel tobb legyen mindket meret es a harmadok szorzata ne legyen kisebb mint 10");
        }//el kell ferjen 10 birka

        _hossz = hossz;
        _szelesseg = szelesseg;
        field = new Object[_hossz][_szelesseg];
        running = false;
        juhok = new ArrayList<>();
        kutyak = new ArrayList<>();

        InitFarm(field);

        BuildWalls(field);
        BuildGates(field);

        SpawnAnimals();
    }

    public Farm() {
        this(15, 15);
    }

    private void InitFarm(Object[][] farm) {
        for (int i = 0; i < farm.length; i++) {
            for (int j = 0; j < farm[0].length; j++) {
                farm[i][j] = new Ures();
            }
        }
    }

    //atmegy a matrix szelen
    private void BuildWalls(Object[][] farm) {

        for (int i = 0; i < farm.length; i++) {
            farm[i][0] = new Fal();
            farm[i][farm[0].length - 1] = new Fal();
        }
        for (int j = 0; j < farm[0].length; j++) {
            farm[0][j] = new Fal();
            farm[farm.length - 1][j] = new Fal();
        }
    }

    private void BuildGates(Object[][] farm) {
        int horLength = farm[0].length;
        int vertLength = farm.length;

        int r1 = RandomNumber(1, horLength - 1);
        int r2 = RandomNumber(1, horLength - 1);
        int r3 = RandomNumber(1, vertLength - 1);
        int r4 = RandomNumber(1, vertLength - 1);

        farm[0][r1] = new Kapu();//fent
        farm[vertLength - 1][r2] = new Kapu();//lent
        farm[r3][0] = new Kapu();//bal
        farm[r4][horLength - 1] = new Kapu();//jobb
    }

    private void SpawnAnimals() {

        //j
        int kozepXkezdet = field.length / 3;
        int kozepXveg = field.length / 3 * 2;
        //i
        int kozepYkezdet = field[0].length / 3;
        int kozepYveg = field[0].length / 3 * 2;

        int birka_num = 0;
        int kutya_num = 0;

        HashSet<String> juh_pozik = new HashSet<>();
        HashSet<String> kutya_pozik = new HashSet<>();

        while (birka_num < 10) {
            int randX = RandomNumber(1, field.length - 1);
            int randY = RandomNumber(1, field[0].length - 1);
            String pozi = randX + "," + randY;

            if (!juh_pozik.contains(pozi)) {

                if ((randX >= kozepXkezdet && randX < kozepXveg) && (randY >= kozepYkezdet && randY < kozepYveg)) {
                    juh_pozik.add(pozi);
                    birka_num++;
                }
            }
        }

        while (kutya_num < 5) {
            int randX = RandomNumber(1, field.length - 1);
            int randY = RandomNumber(1, field[0].length - 1);
            String pozi = randX + "," + randY;

            if (!kutya_pozik.contains(pozi)) {

                if ((randX < kozepXkezdet) || (randX > kozepXveg) || (randY < kozepYkezdet) || (randY > kozepYveg)) {
                    kutya_pozik.add(pozi);
                    kutya_num++;
                }
            }
        }

        int juh_idx = 0;
        for (String pozi : juh_pozik) {

            String[] coords = pozi.split(",");
            int poz_x = Integer.parseInt(coords[0]);
            int poz_y = Integer.parseInt(coords[1]);

            char juh_name = UppercaseLetter(juh_idx);
            Juh juh = new Juh(poz_x, poz_y, this, juh_name);
            juhok.add(juh);
            juh_idx++;
            //field[randX][randY] = juh; tortenik a konstruktoraban
        }

        int kutya_idx = 0;
        for (String pozi : kutya_pozik) {

            String[] coords = pozi.split(",");
            int poz_x = Integer.parseInt(coords[0]);
            int poz_y = Integer.parseInt(coords[1]);

            Kutya kutya = new Kutya(poz_x, poz_y, this, kutya_idx);
            kutyak.add(kutya);
            kutya_idx++;
            //field[randX][randY] = kutya; tortenik a konstruktoraban
        }
    }

    public void StartSimulation() {

        running = true;
        new Nyomtato(this).start();

        for (Juh juh : juhok) {
            juh.start();
        }
        for (Kutya kutya : kutyak) {
            kutya.start();
        }

    }

    //min included, max excluded
    public static int RandomNumber(int min, int max) {
        Random random = new Random();
        return random.nextInt(max - min) + min;
    }

    public static char UppercaseLetter(int number) {
        if (number < 0 || number > 25) {
            throw new IllegalArgumentException("out of abc error");
        }
        return (char) (number + 65);
    }

    public void PrintFarm(Object[][] farm) {
        try {
            lock.readLock().lock();

            for (int i = 0; i < farm.length; i++) {
                for (int j = 0; j < farm[0].length; j++) {
                    System.out.print(farm[i][j] + " ");
                }
                System.out.println("");
            }
        } finally {
            lock.readLock().unlock();
        }
    }

    public ReentrantReadWriteLock getLock() {
        return lock;
    }

    public static void main(String[] args) {

        System.out.println("\033[H\033[2J");
        var far = new Farm(33, 33);
        far.PrintFarm(far.field);
        far.StartSimulation();

    }
}
