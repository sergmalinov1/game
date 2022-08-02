//<PostBuildEvent>copy SharedLibrary.dll $(OutDir)..\..\..\..\..\GameClient\Assets\DLLs\SharedLibrary.dll</PostBuildEvent>

namespace SharedLibrary
{
    public class Player
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int Heals { get; set; }

    }
}