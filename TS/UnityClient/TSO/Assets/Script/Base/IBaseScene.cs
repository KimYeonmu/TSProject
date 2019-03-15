using System.Collections;

interface IBaseScene
{
    string GetSceneName();

    IEnumerable Show();
    IEnumerable Hide();
}