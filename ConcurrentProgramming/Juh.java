package beadandov2;

import java.util.concurrent.locks.ReentrantReadWriteLock;

public class Juh extends Thread {

    //private boolean running;
    private Farm farm;
    private Object toStep2;
    private int x;
    private int y;
    private char name;
    private ReentrantReadWriteLock lock = new ReentrantReadWriteLock(true);

    public Juh(int x, int y, Farm farm, char name) {
        super("" + name);
        this.name = name;

        this.x = x;
        this.y = y;

        this.farm = farm;
        this.lock = farm.getLock();
        //itt rakjuk a matrixba
        try {
            lock.writeLock().lock();
            farm.field[x][y] = this;
        } finally {
            lock.writeLock().unlock();
        }

    }

    @Override
    public void run() {

        while (farm.running) {
            try {//ez itt a mukodese

                if (noDog()) {
                    WanderRandom();
                } else {
                    WanderAvoidDog();
                }

                Thread.sleep(200);

            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }

    }

    @Override
    public String toString() {
        return "" + name;
    }

    private boolean Move(int xdest, int ydest) {
        try {
            lock.writeLock().lock();
            if (IsEmpty(xdest, ydest) || IsGate(xdest, ydest)) {

                //ha kapura fog lepni
                if (IsGate(xdest, ydest)) {
                    farm.running = false;
                }

                farm.field[x][y] = new Ures();
                farm.field[xdest][ydest] = this;
                this.x = xdest;
                this.y = ydest;

                return true;
            } else {
                return false;
            }
        } finally {
            lock.writeLock().unlock();
        }

    }

    private boolean IsGate(int x, int y) {
        return farm.field[x][y] instanceof Kapu;
    }

    private boolean IsEmpty(int x, int y) {
        return farm.field[x][y] instanceof Ures;
    }

    private boolean IsDog(int x, int y) {
        return farm.field[x][y] instanceof Kutya;
    }

    public void WanderRandom() {

        //mindenkeppen mozogjon
        int xmove = Farm.RandomNumber(-1, 2);//-1 1 kozott
        int ymove = Farm.RandomNumber(-1, 2);//-1 1 kozott
        while (xmove == 0 && ymove == 0) {
            xmove = Farm.RandomNumber(-1, 2);
            ymove = Farm.RandomNumber(-1, 2);
        }

        //ha tud menjen
        Move(x + xmove, y + ymove);

    }

    private void WanderAvoidDog() {

        int xmove = 0;
        int ymove = 0;

        if (DogBottom()) {
            xmove = -1;
            ymove = Farm.RandomNumber(-1, 2);

        } else if (DogTop()) {
            xmove = 1;
            ymove = Farm.RandomNumber(-1, 2);

        } else if (DogRight()) {
            xmove = Farm.RandomNumber(-1, 2);
            ymove = -1;

        } else if (DogLeft()) {
            xmove = Farm.RandomNumber(-1, 2);
            ymove = 1;
        } else {
            //ha erzekelt kutyat -> bejon ide -> kutya kozben elment -> fenti esetek nem lesznek jok
            while (xmove == 0 && ymove == 0) {
                xmove = Farm.RandomNumber(-1, 2);
                ymove = Farm.RandomNumber(-1, 2);
            }
        }

        //ha tud menjen
        Move(x + xmove, y + ymove);

    }

    private boolean DogBottom() {
        try {
            lock.readLock().lock();
            return (IsDog(x + 1, y - 1) || IsDog(x + 1, y) || IsDog(x + 1, y + 1));
        } finally {
            lock.readLock().unlock();
        }

    }

    private boolean DogTop() {
        try {
            lock.readLock().lock();
            return (IsDog(x - 1, y - 1) || IsDog(x - 1, y) || IsDog(x - 1, y + 1));
        } finally {
            lock.readLock().unlock();
        }
    }

    private boolean DogLeft() {
        try {
            lock.readLock().lock();
            return (IsDog(x - 1, y - 1) || IsDog(x, y - 1) || IsDog(x + 1, y - 1));
        } finally {
            lock.readLock().unlock();
        }
    }

    private boolean DogRight() {
        try {
            lock.readLock().lock();
            return (IsDog(x - 1, y + 1) || IsDog(x, y + 1) || IsDog(x + 1, y + 1));
        } finally {
            lock.readLock().unlock();
        }
    }

    private boolean noDog() {
        return (!DogBottom() && !DogLeft() && !DogRight() && !DogTop());
    }

}
