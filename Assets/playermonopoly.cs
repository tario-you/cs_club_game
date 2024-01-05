using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerMonopoly : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public GameObject prefabPlatform;
    public Transform transformX;
    public TextMeshPro tmpText;
    private int spawnedPlatforms = 0;
    
    public TMP_InputField inputfield;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI leaderBoardNames;
    public TextMeshProUGUI leaderBoardScores;

    public TextMeshProUGUI livegamescoretext;

    public int score = 0; 

    public Canvas gameOverCanvas;
    public Canvas gameLiveCanvas;
    public string fileName = "scores.txt";
    string filePath;
    
    private float movingspeed = 32f;
    private float movingscale = 0f;

    private bool inputedName = false;

    private void Start(){
        gameOverCanvas.enabled = false;
        inputfield.onEndEdit.AddListener(updateLeaderBoard);
        filePath = Path.Combine(Application.dataPath, fileName);
    }

    public void WriteToFile(string text)
    {
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(text);
        }

        // Debug.Log("Text written to file: " + filePath);
    }

    private void updateLeaderBoard(string inputText){
        if (! inputedName){
            WriteToFile("\n"+inputText+","+score);
            inputedName = true;
            string[] lines = File.ReadAllLines(filePath);
            List<int> scores = new List<int>();
            List<string> names = new List<string>();
            foreach (string line in lines){
                if (line != ""){
                    Debug.Log($"read line {line}");
                    string[] elements = line.Split(',');
                    scores.Add(int.Parse(elements[1]));
                    names.Add(elements[0]);
                }
            }

            // sorting
            List<int> indices = new List<int>();
            for (int i = 0; i < scores.Count; i++)
            {
                indices.Add(i);
            }

            indices.Sort((a, b) => scores[a].CompareTo(scores[b]));

            List<int> sortedscores = new List<int>();
            List<string> sortednames = new List<string>();

            foreach (int index in indices)
            {
                sortedscores.Add(scores[index]);
                sortednames.Add(names[index]);
            }

            scores = sortedscores;
            names = sortednames;

            scores.Reverse();
            names.Reverse();

            // sorting done
            string leaderbordnames = "";
            string leaderbordscores = "";
            for (int i=0;i<scores.Count; i++){
                leaderbordnames += names[i] + "\n";
                leaderbordscores += scores[i] + "\n";
            }
            leaderBoardNames.text = leaderbordnames;
            leaderBoardScores.text = leaderbordscores;
        }
    }

    public void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    
    private void Update()
    {
        PlayerController[] followers = FindObjectsOfType<PlayerController>();
        if (followers.Length == 0){
            gameOverCanvas.enabled = true;
            gameLiveCanvas.enabled = false;
            // scoreText.transform.position = transformX.transform.position;
            scoreText.text = "Score: "+score.ToString();
            ReloadScene();
        }else{
            livegamescoretext.text = "Score: "+score.ToString();
            float selfZ = followers[0].GetComponent<Transform>().position.z;
            // Debug.Log(selfZ.ToString());
            if (selfZ > 0 && selfZ % 40 > 15 && (int)selfZ/(int)40>=spawnedPlatforms){
                // Debug.Log("shud spawn platform");
                Vector3 spawnLoc = new Vector3(0, 0, (spawnedPlatforms+2)*40);
                Instantiate(prefabPlatform, spawnLoc, Quaternion.identity);
                spawnedPlatforms ++;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                SpawnPrefab();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Object.Destroy(followers[0].gameObject);
            }

            tmpText.text = followers.Length.ToString(); 

            if (followers.Length > 0)
            {
                Vector3 averageFollowerPosition = Vector3.zero;
                
                foreach (PlayerController follower in followers)
                {
                    averageFollowerPosition += follower.transform.position;
                }
                
                averageFollowerPosition /= followers.Length;

                Transform closestFollowerTransform = null;
                float closestDistance = Mathf.Infinity;

                foreach (PlayerController follower in followers)
                {
                    float newSpeed = movingspeed + movingscale*Time.time;
                    Debug.Log(newSpeed);
                    float distanceToTarget = Vector3.Distance(new Vector3(movingspeed, averageFollowerPosition.y, averageFollowerPosition.z), follower.transform.position);

                    if (distanceToTarget < closestDistance)
                    {
                        closestDistance = distanceToTarget;
                        closestFollowerTransform = follower.transform;
                    }

                    float horizontalInput = Input.GetAxis("Horizontal");
                    Vector3 moveDirection = (transform.right * horizontalInput).normalized;
                    Vector3 moveVelocity = moveDirection * movingspeed;
                    follower.GetComponent<Rigidbody>().velocity = new Vector3(moveVelocity.x, -movingspeed, newSpeed);
                }

                if (closestFollowerTransform != null)
                {
                    transformX.position = closestFollowerTransform.position;
                    transformX.rotation = closestFollowerTransform.rotation;
                }
            }
        }
    }

    public void SpawnPrefab()
    {
        float randomX = Random.Range(0.5f, 2.5f) * (Random.Range(0, 2) == 0 ? -1 : 1); 
        float randomZ = Random.Range(0.5f, 3.0f) * (Random.Range(0, 2) == 0 ? -1 : 1); 
        float randomY = Random.Range(1.0f, 4.0f); 

        Vector3 modifiedPosition = new Vector3(
            transformX.position.x + randomX,
            transformX.position.y + randomY,
            transformX.position.z + randomZ
        );
        Instantiate(prefabToSpawn, modifiedPosition, Quaternion.identity);
    }
}
