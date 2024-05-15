module PlantsvsZombies where
import Data.Maybe (fromJust, isNothing, isJust)

type Coordinate = (Int, Int)
type Sun = Int
                        --elet
data Plant = Peashooter Int | Sunflower Int | Walnut Int | CherryBomb Int --maradek elet
    deriving (Eq, Show)
                --elet speed
data Zombie = Basic Int Int | Conehead Int Int | Buckethead Int Int | Vaulting Int Int --maradek elet
    deriving (Eq, Show)
data GameModel = GameModel Sun [(Coordinate, Plant)] [(Coordinate, Zombie)]
    deriving (Eq, Show)


{-
________________________________________________________________________________________________________________________________________--
-}

howMuch :: Plant -> Sun
howMuch (Peashooter x) = 100
howMuch (Walnut x) = 50
howMuch (Sunflower x) = 50
howMuch (CherryBomb x) = 150

goodCord :: Coordinate -> Bool
goodCord a
    |fst a < 5 && fst a >= 0 && snd a < 12 && snd a >= 0 = True
    |otherwise = False

tryPurchase :: GameModel -> Coordinate -> Plant -> Maybe GameModel
tryPurchase (GameModel sun plants zombies) c p
    |isNothing (lookup c plants) && (sun >= howMuch p) && goodCord c = Just (GameModel (sun-howMuch p) ((c,p):plants) zombies)
    |otherwise = Nothing
------------------------------------------------------------------------------------------
goodZcord :: Int -> Bool
goodZcord x
    |x>=0 && x<5 = True
    |otherwise = False

notOccupied :: GameModel -> Coordinate -> Bool
notOccupied (GameModel sun plants zombies) c
    |isNothing (lookup c zombies) = True
    |otherwise = False

placeZombieInLane :: GameModel -> Zombie -> Int -> Maybe GameModel
placeZombieInLane (GameModel sun plants zombies) z x
    |goodZcord x && notOccupied (GameModel sun plants zombies) (x,11) = Just (GameModel sun plants (((x,11),z):zombies))
    |otherwise = Nothing

tryPlaceZombie :: GameModel -> Zombie -> Int -> GameModel
tryPlaceZombie (GameModel sun plants zombies) z x
    |goodZcord x && notOccupied (GameModel sun plants zombies) (x,11) = GameModel sun plants (((x,11),z):zombies)
    |otherwise = error "Nemjo xd"

placeZombies :: GameModel -> [(Int, Zombie)] -> GameModel
placeZombies (GameModel sun plants zombies) [] = GameModel sun plants zombies
placeZombies (GameModel sun plants zombies) (newzomb:newzombs) = placeZombies (tryPlaceZombie (GameModel sun plants zombies) (snd newzomb) (fst newzomb)) newzombs

---------------------------------------------------------------------------------------------------------

onPlant :: [(Coordinate, Plant)] -> (Coordinate, Zombie) -> Bool
onPlant plants z
    |isJust (lookup (fst z) plants) = True
    |otherwise = False

cannotJump :: (Coordinate, Zombie) -> Bool
cannotJump (_,Vaulting _ x)
    |x>1 = False
    |otherwise = True
cannotJump (_,_) = True

didJump :: Zombie -> Zombie
didJump (Vaulting x y) = Vaulting x (y-1)
didJump (Basic x y) = Basic x y
didJump (Conehead x y) = Conehead x y
didJump (Buckethead x y) = Buckethead x y

zombiSebesseg :: Zombie -> Int
zombiSebesseg (Basic x y) = y
zombiSebesseg (Conehead x y) = y
zombiSebesseg (Buckethead x y) = y
zombiSebesseg (Vaulting x y) = y

moveZombie :: [(Coordinate, Plant)] -> (Coordinate, Zombie) -> (Coordinate, Zombie)
moveZombie plants ((x,y),z)
    |onPlant plants ((x,y-1),z) = ((x,(y-zombiSebesseg z)),didJump z)--elotte van fix
    |onPlant plants ((x,y),z) =
         case () of
            ()|cannotJump ((x,y),z) -> ((x,y),z)
              |otherwise -> ((x,(y-zombiSebesseg z+1)),didJump z)
    |otherwise = ((x,y-zombiSebesseg z),z)

moveZombies :: [(Coordinate, Plant)] -> [(Coordinate, Zombie)] -> [(Coordinate, Zombie)]
moveZombies plants = map (moveZombie plants)

--zombies
--plants

novenySebzes :: Plant -> Int -> Plant
novenySebzes (Peashooter x) y = Peashooter (max (x-y) 0) -- ne menjen negativ eletbe
novenySebzes (Sunflower x) y = Sunflower (max (x-y) 0)
novenySebzes (Walnut x) y = Walnut (max (x-y) 0)
novenySebzes (CherryBomb x) y = CherryBomb (max (x-y) 0)

damagePlant :: (Coordinate, Plant) -> (Coordinate, Zombie) -> (Coordinate, Plant)
damagePlant (x,p) (y,z)
    |x == y && cannotJump (y,z) = (x,novenySebzes p 1)
    |otherwise = (x,p)

damagePlants :: [(Coordinate, Plant)] -> (Coordinate, Zombie) -> [(Coordinate, Plant)]
damagePlants plants zombie = map (`damagePlant` zombie) plants

allDamagePlants :: [(Coordinate, Plant)] -> [(Coordinate, Zombie)] -> [(Coordinate, Plant)]
allDamagePlants = foldl damagePlants

performHibas :: GameModel -> GameModel
performHibas (GameModel sun plants zombies) = GameModel sun (allDamagePlants plants zombies) (moveZombies plants zombies)

notZombieWon :: (Coordinate, Zombie) -> Bool
notZombieWon ((x,y),z)
    |y<0 = False
    |otherwise = True

validateHibas :: GameModel -> Maybe GameModel
validateHibas (GameModel sun plants zombies)
    |all notZombieWon zombies = Just (GameModel sun plants zombies)
    |otherwise = Nothing

performZombieActions :: GameModel -> Maybe GameModel
performZombieActions x = validateHibas (performHibas x)
--------------------------------------------------------------------------------------
zeroHPP :: Plant -> Bool
zeroHPP (Peashooter x) = x <= 0
zeroHPP (Sunflower x) = x <= 0
zeroHPP (Walnut x) = x <= 0
zeroHPP (CherryBomb x) = x <= 0

cleanPlants :: [(Coordinate, Plant)] -> [(Coordinate, Plant)]
cleanPlants [] = []
cleanPlants (p:ps)
    |zeroHPP (snd p) = cleanPlants ps
    |otherwise = p : cleanPlants ps

zeroHPZ :: Zombie -> Bool
zeroHPZ (Basic x y) = x <= 0
zeroHPZ (Conehead x y) = x <= 0
zeroHPZ (Buckethead x y) = x <= 0
zeroHPZ (Vaulting x y) = x <= 0

cleanZombies :: [(Coordinate, Zombie)] -> [(Coordinate, Zombie)]
cleanZombies [] = []
cleanZombies (z:zs)
    |zeroHPZ (snd z) = cleanZombies zs
    |otherwise = z : cleanZombies zs

cleanBoard :: GameModel -> GameModel
cleanBoard (GameModel sun plants zombies) = GameModel sun (cleanPlants plants) (cleanZombies zombies)
-------------------------------------------------------------------------------------------------

plantDmgType :: (Coordinate, Plant) -> Int
plantDmgType (_,Peashooter x) = 1
plantDmgType (_,Walnut x) = 0
plantDmgType (_,Sunflower x) = 2
plantDmgType (_,CherryBomb x) = 3

dmgZombie :: Int -> (Coordinate, Zombie) -> (Coordinate, Zombie)
dmgZombie x (c,Basic hp speed) = (c,Basic (hp-x) speed)
dmgZombie x (c,Vaulting hp speed) = (c,Vaulting (hp-x) speed)
dmgZombie x (c,Buckethead hp speed) = (c,Buckethead (hp-x) speed)
dmgZombie x (c,Conehead hp speed) = (c,Conehead (hp-x) speed)

adottSorbanZ :: Coordinate -> [(Coordinate, Zombie)] -> [(Coordinate, Zombie)]
adottSorbanZ c = filter (\ z -> snd(fst z) >= snd c && fst(fst z) == fst c) -- jo sorban zombik

sorPoz :: (Coordinate, Zombie) -> Int
sorPoz (c,z) = snd c

zMinSorpozicio :: [(Coordinate, Zombie)] -> Int
zMinSorpozicio [] = -999
zMinSorpozicio z = minimum $ map sorPoz z -- a legkozelebbi adott sorban levo zombi, majd koordinatat atadni

peaShooterDmg :: Coordinate -> [(Coordinate, Zombie)] -> [(Coordinate, Zombie)]
peaShooterDmg cP [] = []
peaShooterDmg cP zombies = map (\z -> if fst(fst z) == fst cP && snd(fst z) == zMinSorpozicio (adottSorbanZ cP zombies) then dmgZombie 1 z else z) zombies
    

getZHP :: Zombie -> Int
getZHP (Basic hp s) = hp
getZHP (Buckethead hp s) = hp
getZHP (Conehead hp s) = hp
getZHP (Vaulting hp s) = hp

cherryDmg :: Coordinate -> [(Coordinate, Zombie)] -> [(Coordinate, Zombie)]
cherryDmg cP [] = []
cherryDmg cP (z:zs)
    | abs((fst(fst z)) - (fst cP)) <= 1 && abs((snd(fst z)) - (snd cP)) <= 1 = dmgZombie (getZHP (snd z)) z : cherryDmg cP zs --attack --3 kell legyen
    | otherwise = z : cherryDmg cP zs

killCherry :: (Coordinate, Plant) -> (Coordinate, Plant)
killCherry (c,CherryBomb x) = (c,CherryBomb 0)

isCherry :: (Coordinate, Plant) -> Bool
isCherry (c,CherryBomb x) = True
isCherry _ = False

cherriesKYS :: [(Coordinate, Plant)]-> [(Coordinate, Plant)]
cherriesKYS [] = []
cherriesKYS (p:ps)
    |isCherry p = killCherry p : cherriesKYS ps
    |otherwise = p : cherriesKYS ps

isSunflower :: (Coordinate, Plant) -> Bool
isSunflower (c,Sunflower x) = True
isSunflower _ = False

sunFN :: [(Coordinate, Plant)] -> Int
sunFN plants = length $ filter isSunflower plants

produceSun :: [(Coordinate, Plant)] -> Sun -> Int
produceSun plants sun = sunFN plants*25+sun

plantDmgZombies :: (Coordinate, Plant) -> [(Coordinate, Zombie)] -> [(Coordinate, Zombie)]
plantDmgZombies p [] = []
plantDmgZombies p z
    |plantDmgType p == 1 = peaShooterDmg (fst p) z
    |plantDmgType p == 2 = z
    |plantDmgType p == 0 = z
    |plantDmgType p == 3 = cherryDmg (fst p) z
    |otherwise = z

plantsDmgZombies :: [(Coordinate, Plant)] -> [(Coordinate, Zombie)] -> [(Coordinate, Zombie)]
plantsDmgZombies [] zombies = zombies
plantsDmgZombies (p:ps) zombies = plantsDmgZombies ps (plantDmgZombies p zombies)

performPlantActions :: GameModel -> GameModel
performPlantActions (GameModel sun plants zombies) = GameModel (produceSun plants sun) (cherriesKYS plants) (plantsDmgZombies plants zombies)

-------------------------------------------------------------------------------------------------------

bFromMB:: Maybe Bool -> Bool
bFromMB (Just True) = True
bFromMB (Just False) = False

gameOver :: GameModel -> Maybe Bool
gameOver (GameModel sun plants zombies)
    |null zombies = Just True
    |not (all notZombieWon zombies) = Just False --nyertek a zombik
    |otherwise = Nothing

add25Sun :: GameModel -> GameModel
add25Sun (GameModel sun plants zombies) = GameModel (sun+25) plants zombies

defendsAgainst :: GameModel -> [[(Int, Zombie)]] -> Bool
defendsAgainst (GameModel sun plants zombies) []
    |isNothing(gameOver (add25Sun(cleanBoard(GameModel sun plants zombies)))) = defendsAgainst (performHibas(cleanBoard(performPlantActions(add25Sun(cleanBoard(GameModel sun plants zombies)))))) []
    |otherwise = bFromMB(gameOver (add25Sun(cleanBoard(GameModel sun plants zombies))))
defendsAgainst (GameModel sun plants zombies) (newzombl:newzombls)
    |isNothing(gameOver (add25Sun(cleanBoard(placeZombies (GameModel sun plants zombies) newzombl)))) = defendsAgainst (performHibas(cleanBoard(performPlantActions(add25Sun(cleanBoard(placeZombies (GameModel sun plants zombies) newzombl)))))) newzombls
    |otherwise = bFromMB(gameOver (add25Sun(cleanBoard(placeZombies (GameModel sun plants zombies) newzombl))))

defendsAgainstI :: (GameModel -> GameModel) -> GameModel -> [[(Int, Zombie)]] -> Bool
defendsAgainstI f (GameModel sun plants zombies) []
    |isNothing(gameOver (add25Sun(cleanBoard(GameModel sun plants zombies)))) = defendsAgainstI f (performHibas(cleanBoard(performPlantActions(f(add25Sun(cleanBoard(GameModel sun plants zombies))))))) [] -- f a performplant mogott volt eddig
    |otherwise = bFromMB(gameOver (add25Sun(cleanBoard(GameModel sun plants zombies))))
defendsAgainstI f (GameModel sun plants zombies) (newzombl:newzombls)
    |isNothing(gameOver (add25Sun(cleanBoard(placeZombies (GameModel sun plants zombies) newzombl)))) = defendsAgainstI f (performHibas(cleanBoard(performPlantActions(f(add25Sun(cleanBoard(placeZombies (GameModel sun plants zombies) newzombl))))))) newzombls
    |otherwise = bFromMB(gameOver (add25Sun(cleanBoard(placeZombies (GameModel sun plants zombies) newzombl))))




-----------------------------------------------------------------------------
defaultPeashooter :: Plant
defaultPeashooter = Peashooter 3

defaultSunflower :: Plant
defaultSunflower = Sunflower 2

defaultWalnut :: Plant
defaultWalnut = Walnut 15

defaultCherryBomb :: Plant
defaultCherryBomb = CherryBomb 2

basic :: Zombie
basic = Basic 5 1

coneHead :: Zombie
coneHead = Conehead 10 1

bucketHead :: Zombie
bucketHead = Buckethead 20 1

vaulting :: Zombie
vaulting = Vaulting 7 2

