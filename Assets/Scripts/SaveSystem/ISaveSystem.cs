namespace DefaultNamespace.SaveSystem
{
    public interface ISaveSystem
    {
        void Save(GameSaveData data);
        GameSaveData Load();
        bool HasSave();
        void Clear();
    }

}