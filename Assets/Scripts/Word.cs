public enum Category
{
    grupo,
    conceito,
    estrutura
}

[System.Serializable]
public class Word
{
    public string word;
    public Category category;
}
