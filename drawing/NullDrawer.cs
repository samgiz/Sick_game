namespace Drawing{
    // draws nothing. used with the server
    public class NullDrawer : Drawer {
        public void DrawDisk(dynamic p){ /* do nothing */ }
        public void DrawBigDisk(dynamic p){ /* do nothing */ }
        public void DrawSegment(dynamic p){ /* do nothing */ }
    }
}