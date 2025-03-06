package beadandov2;

import java.util.concurrent.locks.ReentrantReadWriteLock;

public class Nyomtato extends Thread {

    private Farm farm;
    private ReentrantReadWriteLock lock = new ReentrantReadWriteLock(true);

    public Nyomtato(Farm farm) {
        this.farm = farm;
        this.lock = farm.getLock();
    }

    @Override
    public void run() {
        while (farm.running) {
            try {
                Inform();
                Thread.sleep(200);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }

        Inform();
        DeclareEscapedSheep();
        System.out.println("S I M U L A T I O N   O V E R . . .");

    }

    private void Inform() {

        lock.readLock().lock();
        try {
            System.out.println("\u001B[0;0H");
            farm.PrintFarm(farm.field);
            //System.out.println(farm.running + " " + lock.getReadLockCount());
        } finally {
            lock.readLock().unlock();
        }

    }

    private void DeclareEscapedSheep() {

        for (int i = 0; i < farm.field.length; i++) {
            //top or bottom row
            if (IsSheep(i, 0)) {
                System.out.println(farm.field[i][0] + " has escaped...");
                return;
            } else if (IsSheep(i, farm.field[0].length - 1)) {
                System.out.println(farm.field[i][farm.field[0].length - 1] + " has escaped...");
                return;
            }
        }
        for (int j = 0; j < farm.field[0].length; j++) {
            //left right columns
            if (IsSheep(0, j)) {
                System.out.println(farm.field[0][j] + " has escaped...");
                return;
            } else if (IsSheep(farm.field.length - 1, j)) {
                System.out.println(farm.field[farm.field.length - 1][j] + " has escaped...");
                return;
            }
        }
    }

    private boolean IsSheep(int x, int y) {
        return farm.field[x][y] instanceof Juh;
    }
}
