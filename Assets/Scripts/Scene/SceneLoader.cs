using Phezu.Util;
using UnityEngine.SceneManagement;

namespace Phezu.Derek {

    public class SceneLoader : Singleton<SceneLoader> {

        public void LoadScene(string sceneName) {
            SceneManager.LoadScene(sceneName);
        }
    }
}