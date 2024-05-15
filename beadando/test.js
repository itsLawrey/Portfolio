
//KONSTANSOK
const buttonNew = document.getElementById("button1");
const buttonEnd = document.getElementById("button2");
const buttonRotate = document.getElementById("button-rotate");
const buttonMirror = document.getElementById("button-mirror");
const matrixSize = 11;
const matrixContainer = document.getElementById('matrix-container');
const elemContainer = document.getElementById('elem-container');
const seasonEl = document.getElementById('evszak-mero');
const timeFromSeasonElement = document.getElementById('season-remaining');
const discoveryEl = document.getElementById('discovery-time');


const mA = document.getElementById('mission-A-list-item');
const mB = document.getElementById('mission-B-list-item');
const mC = document.getElementById('mission-C-list-item');
const mD = document.getElementById('mission-D-list-item');

const gameTime = 28; // 7 7 7 7
//tavasz az 28/7 felso egesz resz
//4 tavasz
//3 nyar
//2 osz
//1 tel

let tavaszEl = document.getElementById('season-1');
let nyarEl = document.getElementById('season-2');
let oszEl = document.getElementById('season-3');
let telEl = document.getElementById('season-4');

let remainingGameTime=gameTime;
let evszakValue = 1;
let forma;
let gameOngoing = true;
let missionA;
let missionB;
let missionC;
let missionD;
const missions = 
{
  "basic": [
    {
      "title": "Az erdő széle",
      "description": "A térképed szélével szomszédos erdőmezőidért egy-egy pontot kapsz."
    },
    {
      "title": "Álmos-völgy",
      "description": "Minden olyan sorért, amelyben három erdőmező van, négy-négy pontot kapsz."
    },
    {
      "title": "Krumpliöntözés",
      "description": "A farmmezőiddel szomszédos vízmezőidért két-két pontot kapsz."
    },
    {
      "title": "Határvidék",
      "description": "Minden teli sorért vagy oszlopért 6-6 pontot kapsz."
    }
  ],
  "extra": [
    {
      "title": "Fasor",
      "description": "A leghosszabb, függőlegesen megszakítás nélkül egybefüggő erdőmezők mindegyikéért kettő-kettő pontot kapsz. Két azonos hosszúságú esetén csak az egyikért."
    },
    {
      "title": "Gazdag város",
      "description": "A legalább három különböző tereptípussal szomszédos falurégióidért három-három pontot kapsz."
    },
    {
      "title": "Öntözőcsatorna",
      "description": "Minden olyan oszlopodért, amelyben a farm illetve a vízmezők száma megegyezik, négy-négy pontot kapsz. Mindkét tereptípusból legalább egy-egy mezőnek lennie kell az oszlopban ahhoz, hogy pontot kaphass érte."
    },
    {
      "title": "Mágusok völgye",
      "description": "A hegymezőiddel szomszédos vízmezőidért három-három pontot kapsz."
    },
    {
      "title": "Üres telek",
      "description": "A városmezőiddel szomszédos üres mezőkért 2-2 pontot kapsz."
    },
    {
      "title": "Sorház",
      "description": "A leghosszabb, vízszintesen megszakítás nélkül egybefüggő falumezők mindegyikéért kettő-kettő pontot kapsz."
    },
    {
      "title": "Páratlan silók",
      "description": "Minden páratlan sorszámú teli oszlopodért 10-10 pontot kapsz."
    },
    {
      "title": "Gazdag vidék",
      "description": "Minden legalább öt különböző tereptípust tartalmazó sorért négy-négy pontot kapsz."
    }
  ],
}
const elements = [
    {
        id:1,
        time: 2,
        type: 'water',
        shape: [[1,1,1],
                [0,0,0],
                [0,0,0]],
        rotation: 0,
        mirrored: false
    },
    {
        id:2,
        time: 2,
        type: 'town',
        shape: [[1,1,1],
                [0,0,0],
                [0,0,0]],
        rotation: 0,
        mirrored: false        
    },
    {
        id:3,
        time: 1,
        type: 'forest',
        shape: [[1,1,0],
                [0,1,1],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        id:4,
        time: 2,
        type: 'farm',
        shape: [[1,1,1],
                [0,0,1],
                [0,0,0]],
            rotation: 0,
            mirrored: false  
        },
    {
        id:5,
        time: 2,
        type: 'forest',
        shape: [[1,1,1],
                [0,0,1],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        id:6,
        time: 2,
        type: 'town',
        shape: [[1,1,1],
                [0,1,0],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        id:7,
        time: 2,
        type: 'farm',
        shape: [[1,1,1],
                [0,1,0],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        id:8,
        time: 1,
        type: 'town',
        shape: [[1,1,0],
                [1,0,0],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        id:9,
        time: 1,
        type: 'town',
        shape: [[1,1,1],
                [1,1,0],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        id:10,
        time: 1,
        type: 'farm',
        shape: [[1,1,0],
                [0,1,1],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        id:11,
        time: 1,
        type: 'farm',
        shape: [[0,1,0],
                [1,1,1],
                [0,1,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        id:12,
        time: 2,
        type: 'water',
        shape: [[1,1,1],
                [1,0,0],
                [1,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        id:13,
        time: 2,
        type: 'water',
        shape: [[1,0,0],
                [1,1,1],
                [1,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        id:14,
        time: 2,
        type: 'forest',
        shape: [[1,1,0],
                [0,1,1],
                [0,0,1]],
        rotation: 0,
        mirrored: false  
    },
    {
        id:15,
        time: 2,
        type: 'forest',
        shape: [[1,1,0],
                [0,1,1],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        id:16,
        time: 2,
        type: 'water',
        shape: [[1,1,0],
                [1,1,0],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
]
let chosenElements;
let gameMatrix = createMatrix(11,11,0);
let seasonPoints = [0,0,0,0];
//WHEN ADDING A MISSION: choose, display, evaluate
//FUGGVENYEK
// Delegation: ezt egy nagy dologra kell rakni amiben benne van sok dolog = a parent
// type = event tipus, selector = milyen elemek erdekelnek, handler = a fg a logika
function delegate(parent, type, selector, handler) {
    parent.addEventListener(type, function (event) {
        const targetElement = event.target.closest(selector);

        if (this.contains(targetElement)) {
            handler.call(targetElement, event);
        }
    });
}

function chooseMissions(){
    let tempMissions = missions.basic.map(x => x);
    tempMissions.push(missions.extra[0]);//fasor
    tempMissions.push(missions.extra[1]);//gazdag varos
    tempMissions.push(missions.extra[2]);//ontozocsatorna
    tempMissions.push(missions.extra[3]);//Mágusok völgye
    tempMissions.push(missions.extra[4]);//ures telek
    tempMissions.push(missions.extra[5]);//sorhaz
    tempMissions.push(missions.extra[6]);//paratlan silok
    tempMissions.push(missions.extra[7]);//gazdag videk

    let tempMission = getRandomElement(tempMissions);
    missionA = tempMission;
    tempMissions = tempMissions.filter(x => x.title != tempMission.title);
    
    tempMission = getRandomElement(tempMissions);
    missionB = tempMission;
    tempMissions = tempMissions.filter(x => x.title != tempMission.title);

    tempMission = getRandomElement(tempMissions);
    missionC = tempMission;
    tempMissions = tempMissions.filter(x => x.title != tempMission.title);

    tempMission = getRandomElement(tempMissions);
    missionD = tempMission;
    tempMissions = tempMissions.filter(x => x.title != tempMission.title);

    displayMission(missionA,mA);
    displayMission(missionB,mB);
    displayMission(missionC,mC);
    displayMission(missionD,mD);
}

function displayMission(mission, display){
    let image = display.querySelector('img');
    if(mission.title == "Az erdő széle"){
        image.src = "assets/missions_hun/Group 69.png";     
        //display.innerHTML += `<h3></h3><img src="assets/missions_hun/Group 69.png">`;
    }
    if(mission.title == "Álmos-völgy"){
        image.src = "assets/missions_hun/Group 74.png";  
        //display.innerHTML += `<img src="assets/missions_hun/Group 74.png">`;
    }
    if(mission.title == "Krumpliöntözés"){
        image.src = "assets/missions_hun/Group 70.png";  
        //display.innerHTML += `<img src="assets/missions_hun/Group 70.png">`;
    }
    if(mission.title == "Határvidék"){
        image.src = "assets/missions_hun/Group 78.png";  
        //display.innerHTML += `<img src="assets/missions_hun/Group 78.png">`;
    }
    if(mission.title == "Fasor"){
        image.src = "assets/missions_hun/Group 68.png";  
        //display.innerHTML += `<img src="assets/missions_hun/Group 68.png">`;
    }
    if(mission.title == "Öntözőcsatorna"){
        image.src = "assets/missions_hun/Group 75.png";  
        //display.innerHTML += `<img src="assets/missions_hun/Group 75.png">`;
    }
    if(mission.title == "Mágusok völgye"){
        image.src = "assets/missions_hun/Group 76.png";  
        //display.innerHTML += `<img src="assets/missions_hun/Group 76.png">`;
    }
    if(mission.title == "Üres telek"){
        image.src = "assets/missions_hun/Group 77.png";  
        //display.innerHTML += `<img src="assets/missions_hun/Group 77.png">`;
    }
    if(mission.title == "Páratlan silók"){
        image.src = "assets/missions_hun/Group 73.png";  
        //display.innerHTML += `<img src="assets/missions_hun/Group 73.png">`;
    }
    if(mission.title == "Gazdag vidék"){
        image.src = "assets/missions_hun/Group 79.png";  
        //display.innerHTML += `<img src="assets/missions_hun/Group 79.png">`;
    }
    if(mission.title == "Sorház"){
        image.src = "assets/missions_hun/Group 72.png";  
        //display.innerHTML += `<img src="assets/missions_hun/Group 72.png">`;
    }
    if(mission.title == "Gazdag város"){
        image.src = "assets/missions_hun/Group 71.png";  
        //display.innerHTML += `<img src="assets/missions_hun/Group 71.png">`;
    }
}

function evaluateMission(mission){
    let result = 0;

    if(mission.title == "Az erdő széle"){
        for (let row = 0; row < matrixSize; row++) {
            for (let col = 0; col < matrixSize; col++) {
              if (row === 0 || row === matrixSize-1 || col === 0 || col === matrixSize-1) {// Check if the current element is on the edge
                if(gameMatrix[row][col] == 3){//erdo
                    result ++;
                }
              }
            }
        }
    }

    if(mission.title == "Álmos-völgy"){
        for (let row = 0; row < matrixSize; row++) {
            let erdocount = 0;
            for (let col = 0; col < matrixSize; col++) {
              if (gameMatrix[row][col] == 3) {
                erdocount++;
              }
            }
            if(erdocount == 3){
                result+=4;
            }
        }
    }
    if(mission.title == "Krumpliöntözés"){
        for (let row = 0; row < matrixSize; row++) {
            for (let col = 0; col < matrixSize; col++) {
              if (gameMatrix[row][col] == 4) {// ha viz
                if(
                    (isIndexValid(row-1,col)&&gameMatrix[row-1][col]==2)||
                    (isIndexValid(row+1,col)&&gameMatrix[row+1][col]==2)||
                    (isIndexValid(row,col-1)&&gameMatrix[row][col-1]==2)||
                    (isIndexValid(row,col+1)&&gameMatrix[row][col+1]==2)
                ){//megnezzuk a szomsz farmokat
                    result += 2;
                }
              }
            }
        }
    }
    if(mission.title == "Határvidék"){
        for (let row = 0; row < matrixSize; row++) {
            let full = true;
            for (let col = 0; col < matrixSize; col++) {
                if(isEmpty(row,col)){//vizsgaljuk a sorokat
                    full = false;
                    break;
                    }
            }
            if(full){
               result+=6;
            }
        }
        for (let row = 0; row < matrixSize; row++) {
            let full = true;
            for (let col = 0; col < matrixSize; col++) {
                if(isEmpty(col,row)){//vizsgaljuk az oszlopokat
                    full = false;
                    break;
                }
            }
            if(full){
               result+=6;
            }
        }
    }
    if(mission.title == "Fasor"){
        let longestForestColumnCount=0;
        
        for (let row = 0; row < matrixSize; row++) {
            let forestColumnCount = 0;
            for (let col = 0; col < matrixSize; col++) {
                if(gameMatrix[col][row] == 3){//vizsgaljuk az oszloponkenti erdosorokat DE KELL H OSSZEFUGGO LEGYEN
                    forestColumnCount++;
                }
                else{
                    longestForestColumnCount = Math.max(forestColumnCount,longestForestColumnCount);
                    forestColumnCount=0;
                }
            }
            longestForestColumnCount = Math.max(forestColumnCount,longestForestColumnCount);
        }
        result+=longestForestColumnCount * 2;
    }
    if(mission.title == "Öntözőcsatorna"){
        for (let row = 0; row < matrixSize; row++) {
            let waterCount = 0;
            let farmCount = 0;
            for (let col = 0; col < matrixSize; col++) {
                if(gameMatrix[col][row] == 4){//vizsgaljuk az oszloponkenti farm & viz sorokat
                    waterCount++;
                }
                if(gameMatrix[col][row] == 2){
                    farmCount++;
                }
            }
            if(farmCount == waterCount && farmCount > 0){//legalabb 1 van beloluk
                result+=4;
            }
        }
    }
    if(mission.title == "Mágusok völgye"){
        //lehet hatekonyabb lenne a hegyek 4 szomszedjat nezni mert azok fixek de igy konnyebb
        for (let row = 0; row < matrixSize; row++) {
            for (let col = 0; col < matrixSize; col++) {
              if (gameMatrix[row][col] == 4) {// ha viz
                if(
                    (isIndexValid(row-1,col)&&gameMatrix[row-1][col]==1)||
                    (isIndexValid(row+1,col)&&gameMatrix[row+1][col]==1)||
                    (isIndexValid(row,col-1)&&gameMatrix[row][col-1]==1)||
                    (isIndexValid(row,col+1)&&gameMatrix[row][col+1]==1)
                ){//megnezzuk a szomsz HEGYEKET
                    result += 3;
                }
              }
            }
        }
    }
    if(mission.title == "Üres telek"){
        for (let row = 0; row < matrixSize; row++) {
            for (let col = 0; col < matrixSize; col++) {
              if (gameMatrix[row][col] == 0) {// ha ures
                if(
                    (isIndexValid(row-1,col)&&gameMatrix[row-1][col]==5)||
                    (isIndexValid(row+1,col)&&gameMatrix[row+1][col]==5)||
                    (isIndexValid(row,col-1)&&gameMatrix[row][col-1]==5)||
                    (isIndexValid(row,col+1)&&gameMatrix[row][col+1]==5)
                ){//megnezzuk a szomsz falukat
                    result += 2;
                }
              }
            }
        }
    }
    if(mission.title == "Páratlan silók"){//szemmel 1tol indexelunk szoval itt a paros teli oszlopokat nezzuk
        for (let row = 0; row < matrixSize; row++) {
            let full = true;
            for (let col = 0; col < matrixSize; col+=2) {
                if(isEmpty(col,row)){//vizsgaljuk a oszlopokat
                    full = false;
                    break;
                    }
            }
            if(full){
               result+=10;
            }
        }
    }
    if(mission.title == "Gazdag vidék"){
        for (let row = 0; row < matrixSize; row++) {
            let is1 = false;
            let is2 = false;
            let is3 = false;
            let is4 = false;
            let is5 = false;
            for (let col = 0; col < matrixSize; col++) {
                if(gameMatrix[row][col] == 1){//vizsgaljuk az sorokat, minden tipust sorra
                    is1 = true;
                }
                if(gameMatrix[row][col] == 2){
                    is2 = true;
                }
                if(gameMatrix[row][col] == 3){
                    is3 = true;
                }
                if(gameMatrix[row][col] == 4){
                    is4 = true;
                }
                if(gameMatrix[row][col] == 5){
                    is5 = true;
                }
            }
            if(is1 && is2 && is3 && is4 && is5){
                result+=4;
            }
            
        }
    }
    if(mission.title == "Sorház"){
        let longestVillageCount=0;
        for (let row = 0; row < matrixSize; row++) {
            let villageCount = 0;
            for (let col = 0; col < matrixSize; col++) {
                if(gameMatrix[row][col] == 5){//vizsgaljuk az soronkenti falusorokat DE KELL H OSSZEFUGGO LEGYEN
                    villageCount++;
                }
                else{
                    longestVillageCount = Math.max(villageCount,longestVillageCount);
                    villageCount=0;
                }
            }
            longestVillageCount = Math.max(villageCount,longestVillageCount);
        }//kinyerjuk a leghoszabb sort
        //most megnezzuk vegig hogy hany ilyen van
        for (let row = 0; row < matrixSize; row++) {
            let villageCount = 0;
            for (let col = 0; col < matrixSize; col++) {
              if (gameMatrix[row][col] == 5) {
                villageCount++;
              }
            }
            if(villageCount == longestVillageCount){//ha talalunk egy akkora sort, akkor minden elem 2 pontot er benne
                result+=longestVillageCount*2;
            }
        }
    }
    if(mission.title == "Gazdag város"){
        for (let row = 0; row < matrixSize; row++) {
            for (let col = 0; col < matrixSize; col++) {
                if (gameMatrix[row][col] == 5) {// ha falu
                    let szomszedok = new Set();//veszem sorra a szomszedjait
                    if(isIndexValid(row-1,col)&&!szomszedok.has(gameMatrix[row-1][col])&&gameMatrix[row-1][col]!=0){
                        szomszedok.add(gameMatrix[row-1][col]);
                    }
                    if(isIndexValid(row+1,col)&&!szomszedok.has(gameMatrix[row+1][col])&&gameMatrix[row+1][col]!=0){
                        szomszedok.add(gameMatrix[row+1][col]);
                    }
                    if(isIndexValid(row,col-1)&&!szomszedok.has(gameMatrix[row][col-1])&&gameMatrix[row][col-1]!=0){
                        szomszedok.add(gameMatrix[row][col-1]);
                    }
                    if(isIndexValid(row,col+1)&&!szomszedok.has(gameMatrix[row][col+1])&&gameMatrix[row][col+1]!=0){
                        szomszedok.add(gameMatrix[row][col+1]);
                    }

                    if(szomszedok.size > 2){
                        result+=3;
                    }
                }
            }
        }
    }


    
return result;
}

function CorneredMountainCount(){//mindn evszak vegen amennyit bekeritettunk annyi pont
let c = 0;
for (let row = 0; row < matrixSize; row++) {
    for (let col = 0; col < matrixSize; col++) {
      if (gameMatrix[row][col] == 1) {// ha hegy
        if(//megnezzuk h minden szomszedja e
            (isIndexValid(row-1,col)&&!isEmpty(row-1,col))&&
            (isIndexValid(row+1,col)&&!isEmpty(row+1,col))&&
            (isIndexValid(row,col-1)&&!isEmpty(row,col-1))&&
            (isIndexValid(row,col+1)&&!isEmpty(row,col+1))
        ){
            c += 1;//megszamoljuk hany ilyen hegy van
        }
      }
    }
}
return c;//visszateritjuk, minden evszakvaltasnal hozzaadjuk a ponthoz (a ket kuldetes pontjahoz hozzaadjuk ezt is mielott displayeljuk)
}

function createMatrix(rows, cols, init){
    const matrix = [];
    for (let i = 0; i < rows; i++) {
        const row = [];
        for (let j = 0; j < cols; j++) {
            
            if((i == 1 && j == 1) || (i == 3 && j == 8) || (i == 5 & j == 3) || (i == 8 && j == 9) || (i == 9 && j == 5)){//hegyek
                row.push(init+1);;//jelezzuk a logikaban, hogy hegy van itt, igy krealjuk a matrixot
            }
            else{
                row.push(init);
            }
        }
        matrix.push(row);
    }
    return matrix;
}

function kirajzolTable() {
    
    const table = document.createElement('table');// Készítsünk egy táblázatot
    matrixContainer.innerHTML = ''; // Törölje a korábbi tartalmat
    for (let i = 0; i < matrixSize; i++) {
        const row = document.createElement('tr');
        for (let j = 0; j < matrixSize; j++) {
            const cell = document.createElement('td');
            cell.textContent = '';
            drawTableByType(gameMatrix[i][j],cell);//a fo matrixunk alapjan kirajzoljuk a felhasznalokat a cuccost
            row.appendChild(cell);
        }
        table.appendChild(row);
    }
    matrixContainer.appendChild(table);
}

function kirajzolPreviewForma(){//itt az elem.shape matrix alapjan rajzolok ki valamit, ennek a mintajara meg kellene a main jatekot is olyanra csinalni, hogy egy adat matrixbol rajzoljon ki egy tablazatot html-ben
    if(forma){
    const table1 = document.createElement('table');
    elemContainer.innerHTML='';
    for (let i = 0; i < 3; i++) {
        const row = document.createElement('tr');
        for (let j = 0; j < 3; j++) {
            const cell = document.createElement('td');
            cell.textContent = '';
            drawFormByType(forma,forma.shape[i][j],cell);//attol fuggoen milyen tipusu az elem, kirajzoljuk a formajat
            row.appendChild(cell);
        }
        table1.appendChild(row);
    }
    elemContainer.appendChild(table1);  
    }
    
}

function drawFormByType(elem,index,cell){
    
    cell.removeAttribute('class'); //reset cell for new drawing
    cell.classList.add('cell');

    if(elem.type == 'water'){
        if(index != 0){//itt a kivalasztott elem matrixja alapjan felruhazzuk mintaval
            cell.classList.add('waterCell');
        }
        else{
            cell.classList.add('baseCell');
        }
    }
    if(elem.type == 'town'){
        if(index != 0){
            cell.classList.add('villageCell');
        }
        else{
            cell.classList.add('baseCell');
        }
    }
    if(elem.type == 'forest'){
        if(index != 0){
            cell.classList.add('forestCell');
        }
        else{
            cell.classList.add('baseCell');
        }
    }
    if(elem.type == 'farm'){
        if(index != 0){
            cell.classList.add('plainsCell');
        }
        else{
            cell.classList.add('baseCell');
        }
    }
    
}

function drawTableByType(index,cell){
    //1 - mountain
    //2 - farm
    //3 - forest
    //4 - water
    //5 - town
   
    cell.removeAttribute('class'); //reset cell for new drawing
    cell.classList.add('cell');

    if(index == 0){
        cell.classList.add('baseCell');
    }
    else if(index == 1){
        cell.classList.add('mountainCell');
    }
    else if(index == 2){
        cell.classList.add('plainsCell');
    }
    else if(index == 3){
        cell.classList.add('forestCell');
    }
    else if(index == 4){
        cell.classList.add('waterCell');
    }
    else{ //(index == 5)
        cell.classList.add('villageCell');
    }
}

function updateTime(forma,decreasegametime = true){
    
    if(decreasegametime){
        remainingGameTime -= forma.time;
        remainingGameTime = Math.max(0,remainingGameTime); 
    }
    
    
    if(Math.ceil(remainingGameTime/7) == 4){
        seasonEl.innerText = 'Jelenlegi évszak: tavasz'; // 28 -> 21
        timeFromSeasonElement.innerText = `Évszakból hátralevő idő: ${remainingGameTime - 3*7} / 7`;

        
    } 
    else if(Math.ceil(remainingGameTime/7) == 3)
    {
        seasonEl.innerText = 'Jelenlegi évszak: nyár';
        timeFromSeasonElement.innerText = `Évszakból hátralevő idő: ${remainingGameTime - 2*7} / 7`;
    
        if(evszakValue == 1){//megtortent a tavaszbol valo atjoves ide
            clearMissionStyles();
            setMissionActive(evszakValue+1);
            calculatePoints(evszakValue);
            chosenElements = elements.map(x => x);//ujrakeverjuk a keszletet
        }
        evszakValue = 2;
    }
    else if(Math.ceil(remainingGameTime/7) == 2)
    {
        seasonEl.innerText = 'Jelenlegi évszak: ősz';
        timeFromSeasonElement.innerText = `Évszakból hátralevő idő: ${remainingGameTime - 1*7} / 7`;
    
        if(evszakValue == 2){//megtortent a nyarbol valo atjoves ide
            clearMissionStyles();
            setMissionActive(evszakValue+1);
            calculatePoints(evszakValue);
            chosenElements = elements.map(x => x);//ujrakeverjuk a keszletet
        }
        evszakValue = 3;
    }
    else if(Math.ceil(remainingGameTime/7) == 1)
    {
        seasonEl.innerText = 'Jelenlegi évszak: tél';
        timeFromSeasonElement.innerText = `Évszakból hátralevő idő: ${remainingGameTime} / 7`;
    
        if(evszakValue == 3){//megtortent a oszbol valo atjoves ide
            clearMissionStyles();
            setMissionActive(evszakValue+1);
            calculatePoints(evszakValue);
            chosenElements = elements.map(x => x);//ujrakeverjuk a keszletet
        }
        evszakValue = 4;
    }
    if(remainingGameTime == 0){
        
        endGame();
        
    }
}

function endGame(){
    gameOngoing = false;//jelzi h vege a jateknak
    clearMissionStyles();
    calculatePoints(evszakValue);
    seasonEl.innerText = '';
    discoveryEl.innerText = ``;
    timeFromSeasonElement.innerText = `Vége a játéknak`;
    chosenElements = [];
    forma = {
        id:999,
        time: 0,
        type: 'water',
        shape: [[0,0,0],
                [0,0,0],
                [0,0,0]],
        rotation: 0,
        mirrored: false
    }
    kirajzolPreviewForma()

}

function setMissionActive(n){
    switch (n) {
        case 1:
            mA.classList.add('active-mission');
            mB.classList.add('active-mission');
          break;
        case 2:
            mB.classList.add('active-mission');
            mC.classList.add('active-mission');
          break;
        case 3:
            mC.classList.add('active-mission');
            mD.classList.add('active-mission');
          break;
        case 4:
            mD.classList.add('active-mission');
            mA.classList.add('active-mission');
          break;
        default:
      }
}

function trimForma(matrix) {
    // Find the first non-empty row
    let firstNonEmptyRow = 0;
    while (firstNonEmptyRow < matrix.length) {
        if (matrix[firstNonEmptyRow].some(cell => cell !== 0)) {
            break;
        }
        firstNonEmptyRow++;
    }

    // Find the last non-empty row
    let lastNonEmptyRow = matrix.length - 1;
    while (lastNonEmptyRow >= 0) {
        if (matrix[lastNonEmptyRow].some(cell => cell !== 0)) {
            break;
        }
        lastNonEmptyRow--;
    }

    // Find the first non-empty column
    let firstNonEmptyCol = 0;
    while (firstNonEmptyCol < matrix[0].length) {
        if (matrix.some(row => row[firstNonEmptyCol] !== 0)) {
            break;
        }
        firstNonEmptyCol++;
    }

    // Find the last non-empty column
    let lastNonEmptyCol = matrix[0].length - 1;
    while (lastNonEmptyCol >= 0) {
        if (matrix.some(row => row[lastNonEmptyCol] !== 0)) {
            break;
        }
        lastNonEmptyCol--;
    }

    // Extract the trimmed matrix
    const trimmedMatrix = [];
    for (let i = firstNonEmptyRow; i <= lastNonEmptyRow; i++) {
        const trimmedRow = matrix[i].slice(firstNonEmptyCol, lastNonEmptyCol + 1);
        trimmedMatrix.push(trimmedRow);
    }

    return trimmedMatrix;
}

function clearMissionStyles(){
    mA.classList.remove('active-mission');
    mB.classList.remove('active-mission');
    mC.classList.remove('active-mission');
    mD.classList.remove('active-mission');
}

function calculatePoints(n){
    switch (n) {
        case 1:
        
          displaySeasonPoints(evaluateMission(missionA) + evaluateMission(missionB) + CorneredMountainCount());
          //kuldeteseket is ki kell irni. 
          displayMissionPoints(missionA,evaluateMission(missionA));
          displayMissionPoints(missionB,evaluateMission(missionB));
          break;
        case 2:
          displaySeasonPoints(evaluateMission(missionB) + evaluateMission(missionC) + CorneredMountainCount());
          
          displayMissionPoints(missionB,evaluateMission(missionB));
          displayMissionPoints(missionC,evaluateMission(missionC));
          break;
        case 3:
          displaySeasonPoints(evaluateMission(missionC) + evaluateMission(missionD) + CorneredMountainCount());
          
          displayMissionPoints(missionC,evaluateMission(missionC));
          displayMissionPoints(missionD,evaluateMission(missionD));
          break;
        case 4:
          displaySeasonPoints(evaluateMission(missionA) + evaluateMission(missionD) + CorneredMountainCount());
          
          displayMissionPoints(missionD,evaluateMission(missionD));
          displayMissionPoints(missionA,evaluateMission(missionA));
          break;
        default:
          alert("ismeretlen evszak, valami nagyon nemjo");
      }
}

function getSeasonText(n){
    switch (n) {
        case 1://tavasz
           return 'Tavasz';
        case 2://nyar
            
          return 'Nyár';
        case 3://osz
            
          return 'Ősz';
        case 4://tel
            
            return 'Tél';
        default:
    }
}

function displayMissionPoints(mission,points){
    console.log(mission,points);

    let text = `${getSeasonText(evszakValue)}: ${points} pont`;
    if(mission == missionA){
        mA.innerHTML += `<h4>${text}</h4>`;
    }
    if(mission == missionB){
        mB.innerHTML += `<h4>${text}</h4>`;
    }
    if(mission == missionC){
        mC.innerHTML += `<h4>${text}</h4>`;
    }
    if(mission == missionD){
        mD.innerHTML += `<h4>${text}</h4>`;
    }
}

function displaySeasonPoints(points){
    let elem = document.getElementById(`season-${evszakValue}`);
    seasonPoints[evszakValue-1] = points;//evszak pontok arraybe megfelelo helyre irunk pontokat
    if(elem){
        elem.innerText=`${points} pont`;
    }
    if(evszakValue==4 || !gameOngoing){
        let elem = document.getElementById('game-points');
        elem.innerText=`Összpont: ${seasonPoints.reduce((accumulator, currentValue) => accumulator + currentValue, 0)}`;
    }

}

function getRandomElement(array) {
    const randomIndex = Math.floor(Math.random() * array.length);
    return array[randomIndex];
}

function isEmpty(x,y){
    return gameMatrix[x][y] == 0
}

function updateFelfedezes(forma){
    if(forma){
        discoveryEl.innerText = `Felfedezési idő: ${forma.time}`;
    }else{
        discoveryEl.innerText = ``;
    }
    
}

function placeForma(elem,x,y){//mit honnan kezdve rakjuk le

updateMatrix(elem,x,y);
kirajzolTable();



}

function updateMatrixHelpByType(x,y,type){
    //1 - mountain
    //2 - farm
    //3 - forest
    //4 - water
    //5 - town
    if(type == 'water'){
        gameMatrix[x][y] = 4;
    }
    else if(type == 'town'){
        gameMatrix[x][y] = 5;
    }
    else if(type == 'farm'){
        gameMatrix[x][y] = 2;
    }
    else {//(type == 'forest'){
        gameMatrix[x][y] = 3;
    }
}

function updateMatrix(elem,x,y){
    const miniMX = trimForma(elem.shape);
    const type = elem.type;
    console.log('miniMX',miniMX);
    //x meg y hogy honnan indultunk
    //es akkor x+i meg y+j lesz

    //console.log(miniMX);
    //console.log(type);

    for(let i = 0 ; i < miniMX.length ; i++){//tehat a matrixon belul innen lepkedunk :)))
        for(let j = 0 ; j < miniMX[0].length ; j++){
            if(miniMX[i][j]!=0){//tehat azt nezem h a kicsi matrix ahol nem nulla ott van alak es ott kell valtoztatni a nagyot is
                //itt meg lesz a sok if hogy milyen szaot irjunk be a TYPE alapjan
                updateMatrixHelpByType(i+x,j+y,type);
            }
            // 0 1 1
            // 0 1 0
            // 0 0 1
        }
    }
    //console.log(gameMatrix);
}

function chooseFormToDisplay(){
    forma = getRandomElement(chosenElements);
    chosenElements = chosenElements.filter(x => x.id != forma.id)//amit kivalasztottunk azt kiszedjuk h ujra ne valasszuk

    updateFelfedezes(forma);//az uj leRAKANDO formanak irjuk ki a koltseget
    kirajzolPreviewForma();//megmutatjuk mi ez
}

function isIndexValid(i, j) {
    if (i >= 0 && i < gameMatrix.length) {
        if (j >= 0 && j < gameMatrix[i].length) {
            return true;
        }
    }
    return false;
}

function rotateMatrix(matrix) {
    for (let i = 0; i < 3; i++) {
        for (let j = i + 1; j < 3; j++) {
            [matrix[i][j], matrix[j][i]] = [matrix[j][i], matrix[i][j]];
        }
    }
    for (let i = 0; i < 3; i++) {
        matrix[i].reverse();
    }

    return matrix;
}

function mirrorMatrix(matrix) {
    for (let i = 0; i < 3; i++) {
        matrix[i].reverse();
    }

    return matrix;
}

function CanPlace(forma,x,y){
    let miniMX = trimForma(forma.shape);
    for(let i = 0 ; i < miniMX.length ; i++){
        for(let j = 0 ; j < miniMX[0].length ; j++){
            
            if(miniMX[i][j] == 1){//ahol a forma van OTT kell ures legyen
                if(!isIndexValid(i+x,j+y)){//valid index
                    return false;
                    }
                if(!isEmpty(i+x,j+y)){
                    return false;
                }
                
            }
            
        }
    }
    return true;
}

function saveGameState(){
    //save model
    let gameData ={
        remainingGameTime,
        evszakValue,
        forma,
        gameOngoing,
        missionA,
        missionB,
        missionC,
        missionD,
        chosenElements,
        gameMatrix,
        seasonPoints,
        szovegA:mA.innerHTML,
        szovegB:mB.innerHTML,
        szovegC:mC.innerHTML,
        szovegD:mD.innerHTML,
    };
    localStorage.setItem('jatek',JSON.stringify(gameData));
}

function loadGameState(){
    
    const savedGame = localStorage.getItem('jatek');
    if(savedGame){
        let gameData = JSON.parse(savedGame);
        let szovegA;
        let szovegB;
        let szovegC;
        let szovegD;
        
        ({remainingGameTime,
        evszakValue,
        forma,
        gameOngoing,
        missionA,
        missionB,
        missionC,
        missionD,
        chosenElements,
        gameMatrix,
        seasonPoints,
        szovegA,
        szovegB,
        szovegC,
        szovegD,
        } = gameData);
        
        
        kirajzolTable();
        kirajzolPreviewForma();
        updateFelfedezes(forma);
        updateTime(forma,false);

        if(gameOngoing){
            setMissionActive(evszakValue);
        }
        else{
            let elem = document.getElementById('game-points');
            elem.innerText=`Összpont: ${seasonPoints.reduce((accumulator, currentValue) => accumulator + currentValue, 0)}`;
            seasonEl.innerText = '';
            discoveryEl.innerText = ``;
            timeFromSeasonElement.innerText = `Vége a játéknak`;
        }

        displayMission(missionA,mA);
        displayMission(missionB,mB);
        displayMission(missionC,mC);
        displayMission(missionD,mD);

        mA.innerHTML = szovegA;
        mB.innerHTML = szovegB;
        mC.innerHTML = szovegC;
        mD.innerHTML = szovegD;

        tavaszEl.innerText=`${seasonPoints[0]??0} pont`;
        nyarEl.innerText=`${seasonPoints[1]??0} pont`;
        oszEl.innerText=`${seasonPoints[2]??0} pont`;
        telEl.innerText=`${seasonPoints[3]??0} pont`;

        
    }
    else{
        newGame();
    }
    
    
}

function newGame(){
    
    remainingGameTime=gameTime;
    evszakValue = 1;
    gameOngoing = true;
    gameMatrix = createMatrix(11,11,0);
    seasonPoints = [0,0,0,0];
    chosenElements = elements.map(x => x);//elso keveres
    //kiurites:
    mA.innerHTML=`<h3 id="missionA-header">A.</h3><img>`;
    mB.innerHTML=`<h3 id="missionB-header">B.</h3><img>`;
    mC.innerHTML=`<h3 id="missionC-header">C.</h3><img>`;
    mD.innerHTML=`<h3 id="missionD-header">D.</h3><img>`;

    chooseMissions();
    setMissionActive(evszakValue);
    kirajzolTable();
    chooseFormToDisplay();

    let elem = document.getElementById('game-points');
    elem.innerText=`Összpont: `;
    tavaszEl.innerText=`0 pont`;
    nyarEl.innerText=`0 pont`;
    oszEl.innerText=`0 pont`;
    telEl.innerText=`0 pont`;
    updateTime(forma,false);

    saveGameState();
}


//EVENTEK
buttonNew.addEventListener('click',e => {
    newGame();
})

buttonEnd.addEventListener('click',e => {
    if(gameOngoing){
        endGame();
        saveGameState();
    }
    else{
        alert('A játéknak már vége.');
    }
})

buttonRotate.addEventListener('click',e => {
    if(gameOngoing){
        forma.shape = rotateMatrix(forma.shape);
        kirajzolPreviewForma();
    }
    else{
        alert('A játéknak már vége.');
    }
    
})

buttonMirror.addEventListener('click',e => {
    if(gameOngoing){
        forma.shape = mirrorMatrix(forma.shape);
        kirajzolPreviewForma(); 
    }
    else{
        alert('A játéknak már vége.');
    }
    
})




//JATEKMENET, HIVASOK

delegate(matrixContainer,'click','.cell',function (event) {//delegate vegig reagal a klikkelesre
    let clickedRow = event.target.parentElement.rowIndex;
    let clickedColumn = event.target.cellIndex ;
    if(gameOngoing){
        if(CanPlace(forma,clickedRow,clickedColumn)){
        placeForma(forma, clickedRow, clickedColumn);
        updateTime(forma);//a regi forma szerint updateoljuk az idot
        chooseFormToDisplay();
        }
        else{
            window.alert("Ide nem lehet lerakni az elemet.");
        }
    }
    else{
        alert('A játéknak már vége, kezdj újat.')
    }
    saveGameState();
        
})
//betoltes
loadGameState();