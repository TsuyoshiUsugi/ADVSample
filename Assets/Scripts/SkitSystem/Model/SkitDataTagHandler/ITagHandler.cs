namespace SkitSystem.Model.SkitDataTagHandler
{
    public interface ISkitTagHandler
    {
        /// <summary>
        /// このハンドラが対応するタグ名（例："flag"）
        /// </summary>
        string HandleTagName { get; }

        /// <summary>
        /// タグの値を受け取り処理を行う
        /// </summary>
        void Handle(string value);
    }
}
