package beadandov2;

import java.util.concurrent.locks.ReentrantReadWriteLock;

public class Kutya extends Thread {

    private int name;
    private int x;
    private int y;
    private Farm farm;
    private ReentrantReadWriteLock lock = new ReentrantReadWriteLock(true);

    public Kutya(int x, int y, Farm farm, int name) {
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
            try {
                Wander();
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

    public void Wander() {

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

    public boolean DestinationOutOfMiddle(int xdest, int ydest) {
        //j
        int kozepXkezdet = farm.field.length / 3;
        int kozepXveg = farm.field.length / 3 * 2;
        //i
        int kozepYkezdet = farm.field[0].length / 3;
        int kozepYveg = farm.field[0].length / 3 * 2;

        if ((xdest < kozepXkezdet) || (xdest > kozepXveg) || (ydest < kozepYkezdet) || (ydest > kozepYveg)) {
            return true;
        } else {
            return false;
        }
    }

    public boolean Move(int xdest, int ydest) {

        try {
            lock.writeLock().lock();

            if (IsEmpty(xdest, ydest) && DestinationOutOfMiddle(xdest, ydest)) {

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

    private boolean IsEmpty(int x, int y) {
        return farm.field[x][y] instanceof Ures;
    }
}
