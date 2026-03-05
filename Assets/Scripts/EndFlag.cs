using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;

public class EndFlag : MonoBehaviour
{
    public string SceneToLoad;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(SceneToLoad, LoadSceneMode.Single);
        //LoadScene(SceneToLoad);
    }

    /*IEnumerator LoadScene(string scenePath)
    {
        // load the scene in the background while the current scene runs
        // this allows us to add loading screens if need be

        AsyncOperation asyncLoad = SceneManager.LoadScene(scenePath, LoadSceneMode.Single);

        // do nothing until scene is loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }*/

    private void OnDrawGizmos()
    {
        Color cc = new Color(0, 1f, 0.3f, 0.5f);
        Gizmos.color = cc;
        Gizmos.DrawSphere(transform.position, gameObject.GetComponent<SphereCollider>().radius);
    }
}
