namespace MemoryEngine;

public record ImageId(int Value) {
    private static int currentId = 0;
    private static int ShouldChange = 0;
    private static object syncLock = new object();

    public static int GetImageId() {
        if (ShouldChange == 0) {
            ShouldChange += 1;
            return currentId;
        }
        else if (ShouldChange == 1) {
            ShouldChange += 1;
            return currentId;
        }
        else {
            ShouldChange = 1;
            currentId += 1;
            return currentId;
        }
    }
}