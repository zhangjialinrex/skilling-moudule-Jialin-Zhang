using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SpatialSlur.SlurCore;
using SpatialSlur.SlurField;
using SpatialSlur.SlurUnity;
public class Environment : MonoBehaviour {

    // VARIABLES

    public GameObject GUI_InterfaceRef;
    

    // Texture to be used as start of CA input

    Texture2D seedImage;

  
    GOLRule usingRule;

    // Number of frames to run which is also the height of the CA
    //public int timeEnd = 2;
    
    int currentFrame = 0;

    //variables for size of the 3d grid
    int width;
    int length;
    int height;
    int timeEnd;
    // Array for storing voxels
    GameObject[,,] voxelGrid;//
    private List<GameObject> VoxelGridList = new List<GameObject>();

    // Reference to the voxel we are using
    public GameObject voxelPrefab;

    // Spacing between voxels
    float spacing = 1.0f;

    //Layer Densities
    int totalAliveCells = 0;
    float layerdensity = 0;
    float layerdensitySum = 0;
    float[] layerDensities;//array
    float maxlayerdensity = 0;
    float minlayerdensity = 1f;
    float maxVNdensity = 0;
    float minVNdensity = 6f;

    private bool pause = false;
     bool clearedScene = false;
     bool startedGameRf = false;
     bool stoppedGameRf = false;
     bool selectedSeedImageRf = false;
      bool selectedRuleRf = false;
    

    //Max Age
    int maxAge = 0;

    //Max Densities
    int maxDensity3dMO = 0;
    int minDensity3dMO = 50;
    int minDensity3dVN = 50;
    int maxDensity3dVN = 0;


    // Setup Different Game of Life Rules
    GOLRule deathrule = new GOLRule();


    bool GoingToDead = false;
    bool GoingToFull = false;
    bool GoingToStay = false;
    //boolean switches
    //toggles pausing the game

    int vizmode = 0;

    float averageVNdensity = 0.000f;
    float averageLayerdensity = 0.000f;
    ////////++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //public Rigidbody Prefab;
    //public Color[] Spectrum;

    //[Range(1, 100)]
    //public int CountX = 20;

    //[Range(1, 100)]
    //public int CountY = 20;

    //[Range(1, 100)]
    //public int CountZ = 40;

    //[Range(0.0f, 10000.0f)]
    //public float MaxForce = 1000.0f;

    //[Range(0.0f, 10000.0f)]
    //public float MaxTorque = 1000.0f;

    //private float BreakForce = Mathf.Infinity;
    //private float BreakTorque = Mathf.Infinity;

    //private Rigidbody[] _bodies;
    //private Material[] _materials;
    //private FixedJoint[][] _joints; // TO DO maintain array of joints per body, colour bodies by average torque/force



    //////////++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    

    public void ClearScene()
    {
        //  GameObject
        // if (GUI_Interface) {
        currentFrame = 0;
        foreach (GameObject voxel in voxelGrid)
            {
                Destroy(voxel);
                VoxelGridList.Remove(voxel);
             Destroy(gameObject.GetComponent<ModelDisplay>());
            }
            //clearedScene = true;
       // }
    }


    



    // FUNCTIONS
    int index = 0;
    int countframe = 0;
    int countframe2 = 0;
   // int getRuleNum;
    // Use this for initialization
    void Start()
    {


        // Read the image width and height
        //seedImage = SeedImage[0];
        //width = seedImage.width;
        //length = seedImage.height;
        timeEnd = GUI_InterfaceRef.GetComponent<GUI_Interface>().GetTimeEnd();
        
        

        //getRuleNum = GUI_Interface.GetRuleNum();
        //Setup GOL Rules


        //Layer Densities
        //layerDensities Array

        // Create a new CA grid
        // CreateGrid();
        // SetupNeighbors3d();
        
        ///////////////////////////////////////+++++++++++++++++++++++++++++++++++++++++

        //CreateJoints();
        //CacheMaterials();

        //Fix(LayerXZ(0));//???
        ////Fix(CornersXZ());
        ////Fix(Box(0, 0, 0, 3, 3, 1));
        ////Fix(Box(CountX - 3, CountY - 3, CountZ - 1, CountX, CountY, CountZ));

        //StartCoroutine(UpdateBodyColors());
        /////////////////////////////++++++++++++++++++++++++++++++++++++++++++++++++++++

    }

    // Update is called once per frame
    void Update()        
    {
        if (Input.GetKey(KeyCode.A))
        {

            Resolution[] resolutions = Screen.resolutions;

            Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);

            Screen.fullScreen = true;
            if (Screen.fullScreen == true)
            {
                Screen.fullScreen = false;
            }
        }

        startedGameRf = GUI_InterfaceRef.GetComponent<GUI_Interface>().GetStartGame();
        stoppedGameRf = GUI_InterfaceRef.GetComponent<GUI_Interface>().GetStopGame();
        selectedSeedImageRf = GUI_InterfaceRef.GetComponent<GUI_Interface>().GetSelectedSeedImage();
        selectedRuleRf = GUI_InterfaceRef.GetComponent<GUI_Interface>().GetSelectedRule();
       

        if (stoppedGameRf == true)
        {
            ClearScene();
        }
        if (VoxelGridList.Count > 0)
        {
            clearedScene = false;
        }

        if (VoxelGridList.Count == 0)
        {
            clearedScene = true;
        }
      
            if (startedGameRf== true && clearedScene == true &&selectedSeedImageRf == true &&selectedRuleRf == true)
            {
            width = GUI_InterfaceRef.GetComponent<GUI_Interface>().SeedImageWidth();
            length= GUI_InterfaceRef.GetComponent<GUI_Interface>().SeedImageLength();
            timeEnd = GUI_InterfaceRef.GetComponent<GUI_Interface>().GetTimeEnd();
            height = timeEnd;
            layerDensities = new float[timeEnd];
            
           
            CreateGrid();
            SetupNeighbors3d();

            }


            // Calculate the CA state, save the new state, display the CA and increment time frame
            if (VoxelGridList.Count > 0)
            {
               if (currentFrame < timeEnd - 1)
                {

                   if (pause == false)
                   {

                    // Calculate the future state of the voxels
                    CalculateCA();
                    // Update the voxels that are printing
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < length; j++)
                        {
                            GameObject currentVoxel = voxelGrid[i, j, 0];
                            currentVoxel.GetComponent<Voxel>().UpdateVoxel();
                            
                        }

                    }
                    // Save the CA state
                    SaveCA();

                    //Update 3d Densities
                    updateDensities3d();
                    AverageVNdensity();
                    // GetDensity();
                    //print averageV

                    // Increment the current frame count
                    currentFrame++;

                }


            }

            // Display the printed voxels
            // show different visual modes
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    for (int k = 1; k < currentFrame; k++)
                    {
                        if (vizmode == 0)
                        {
                            
                            voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplayLayerDensity(layerDensities[k], minlayerdensity, maxlayerdensity);//////////
                            
                        }
                        if (vizmode == 1)
                        {
                            voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplayEachColor();

                        }

                        if (vizmode == 2)
                        {
                            voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplayDensity3dVN(minDensity3dVN, maxDensity3dVN);

                        }
                        if (vizmode == 3)
                        {
                            voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplayDensity3dMO(minDensity3dMO, maxDensity3dMO);
                            

                        }
                        if (vizmode == 4)
                        {
                            
                            voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplayAge(maxAge);

                        }


                    }
                }
            }

            KeyPressMethod();

        
       }

    }

    /// <summary>
    /// Keies the press method.
    /// 
    /// </summary>
    /// 
    public void AutoChangeMode(float _timeInGUI) {
        
        if (_timeInGUI > 40f)
        {
            if (vizmode <= 4)
            {
                vizmode++;
            }
            if (vizmode > 4)
            {
                vizmode = 0;
            }
            
        }
    }
    public void Spinning() {
        if (gameObject.GetComponent<ModelDisplay>() == null)
        {
            gameObject.AddComponent<ModelDisplay>();
        }
        else
        {
            Destroy(gameObject.GetComponent<ModelDisplay>());
        }
    }
    public void KeyPressMethod()
    {
        // Spin the CA if spacebar is pressed (be careful, GPU instancing will be lost!)
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (gameObject.GetComponent<ModelDisplay>() == null)
            {
                gameObject.AddComponent<ModelDisplay>();
            }
            else
            {
                Destroy(gameObject.GetComponent<ModelDisplay>());//这是什么意思？？？
            }
        }

        //toggle pause with "p" key
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pause == false)
            {
                pause = true;
            }
            else
            {
                pause = false;
            }
        }

        //toggle pause with "p" key
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (vizmode <= 4)
            {
                vizmode++;
            }
            if (vizmode > 4)
            {
                vizmode = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExportPrepare();
        }
    }

    // Create grid function
    void CreateGrid()
    {
        
        // Allocate space in memory for the array
        voxelGrid = new GameObject[width, length, height];
        // Populate the array with voxels from a base image
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 0; k < height; k++)
                {



                    // Create values for the transform of the new voxel
                    Vector3 currentVoxelPos = new Vector3(i * spacing, k * spacing, j * spacing);
                    Quaternion currentVoxelRot = Quaternion.identity;

                    //create the game object of the voxel
                    GameObject currentVoxelObj = Instantiate(voxelPrefab, currentVoxelPos, currentVoxelRot);

                    //run the setupVoxel() function inside the 'Voxel' component of the voxelPrefab
                    //this sets up the instance of Voxel class inside the Voxel game object
                    currentVoxelObj.GetComponent<Voxel>().SetupVoxel(i, j, k, 1);
                   
                    


                    // Set the state of the voxels
                    if (k == 0)
                    {
                        // Create a new state based on the input image
                        seedImage= GUI_InterfaceRef.GetComponent<GUI_Interface>().GetseedImage();
                        float t = seedImage.GetPixel(i, j).grayscale;

                        // black - > alive
                        if (t > 0.5f)
                            currentVoxelObj.GetComponent<Voxel>().SetState(0);
                        else
                            currentVoxelObj.GetComponent<Voxel>().SetState(1);

                    }
                    else
                    {
                        // Set the state to death
                        currentVoxelObj.GetComponent<MeshRenderer>().enabled = false;
                        currentVoxelObj.GetComponent<Voxel>().SetState(0);
                    }
                    // Save the current voxel in the voxelGrid array
                    voxelGrid[i, j, k] = currentVoxelObj;
                    // Attach the new voxel to the grid game object
                    currentVoxelObj.transform.parent = gameObject.transform;
                    VoxelGridList.Add(currentVoxelObj);

                }
            }
        }


        //////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //_bodies = new Rigidbody[CountX * CountY * currentFrame];
        //int index = 0;

        //for (int k = 0; k < currentFrame; k++)
        //{
        //    for (int j = 0; j < CountY; j++)
        //    {
        //        for (int i = 0; i < CountX; i++, index++)
        //        {

        //            var p = new Vector3(i, j, k);
        //            var body = Instantiate(Prefab, transform);
        //            body.transform.localPosition = p;

        //            _bodies[index] = body;
        //        }
        //    }
        //}
        ////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


    }



    // Calculate CA function
    void CalculateCA()
    {
        // Go over all the voxels stored in the voxels array
        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 1; j < length - 1; j++)
            {
                GameObject currentVoxelObj = voxelGrid[i, j, 0];
                int currentVoxelState = currentVoxelObj.GetComponent<Voxel>().GetState();
                int aliveNeighbours = 0;

                // Calculate how many alive neighbours are around the current voxel
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        GameObject currentNeigbour = voxelGrid[i + x, j + y, 0];
                        int currentNeigbourState = currentNeigbour.GetComponent<Voxel>().GetState();
                        aliveNeighbours += currentNeigbourState;
                    }
                }
                aliveNeighbours -= currentVoxelState;

                // GameObject ruleclass= GUI.GetComponent<GUI_Interface>().GetRule();
                //CHANGE RULE BASED ON CONDITIONS HERE:
                GOLRule currentRule = GUI_InterfaceRef.GetComponent<GUI_Interface>().GetRule();
                //CHANGE RULE BASED ON CONDITIONS HERE:
                //..........
                
                



                if (currentFrame < 40)
                {
                    if (layerdensity < 0.05)
                    {
                        GoingToDead = true;
                        GoingToFull = false;
                        GoingToStay = false;
                    }
                    if (layerdensity >= 0.05 && layerdensity < 0.25)
                    {
                        GoingToDead = false;
                        GoingToFull = false;
                        GoingToStay = true;
                    }
                    if (layerdensity >= 0.25)
                    {
                        GoingToDead = false;
                        GoingToFull = true;
                        GoingToStay = false;
                    }
                    if (GoingToStay ==true&& averageVNdensity < 1.5) {///切成VN更大的且能maintain的rule
                        GUI_InterfaceRef.GetComponent<GUI_Interface>().ChangeRule(3);
                        currentRule = GUI_InterfaceRef.GetComponent<GUI_Interface>().ReturnChangeRule();
                     }
                    if (GoingToDead == true) {///切成增长最大的rule，无论VN
                        GUI_InterfaceRef.GetComponent<GUI_Interface>().ChangeRule(7);
                        currentRule = GUI_InterfaceRef.GetComponent<GUI_Interface>().ReturnChangeRule();
                    }
                    if (GoingToDead == true && averageVNdensity < 1.5) {///切成增长第二且VN大的rule
                        GUI_InterfaceRef.GetComponent<GUI_Interface>().ChangeRule(1);
                        currentRule = GUI_InterfaceRef.GetComponent<GUI_Interface>().ReturnChangeRule();
                    }
                    if (GoingToFull == true){///切成变少最快的rule，无论VN
                        GUI_InterfaceRef.GetComponent<GUI_Interface>().ChangeRule(5);
                        currentRule = GUI_InterfaceRef.GetComponent<GUI_Interface>().ReturnChangeRule();
                    }
                    if (GoingToFull == true && averageVNdensity < 1.5) {///切成变少第二快且VN大的rule
                        GUI_InterfaceRef.GetComponent<GUI_Interface>().ChangeRule(2);
                        currentRule = GUI_InterfaceRef.GetComponent<GUI_Interface>().ReturnChangeRule();
                    }


                }
                if (currentFrame >=40)
                {
                    if (layerdensity < 0.25)
                    {
                        GoingToDead = true;
                        GoingToFull = false;
                        GoingToStay = false;
                    }
                    if (layerdensity >= 0.25 && layerdensity < 0.3)
                    {
                        GoingToDead = false;
                        GoingToFull = false;
                        GoingToStay = true;
                    }
                    if (layerdensity >= 0.3)
                    {
                        GoingToDead = false;
                        GoingToFull = true;
                        GoingToStay = false;
                    }
                    if (GoingToStay == true) {//选增长稍大的rule
                        GUI_InterfaceRef.GetComponent<GUI_Interface>().ChangeRule(6);
                        currentRule = GUI_InterfaceRef.GetComponent<GUI_Interface>().ReturnChangeRule();
                    }
                    if (GoingToStay == true&& averageVNdensity > 2) {//选VN最小的，且增长稍大的rule
                        GUI_InterfaceRef.GetComponent<GUI_Interface>().ChangeRule(8);
                        currentRule = GUI_InterfaceRef.GetComponent<GUI_Interface>().ReturnChangeRule();
                    }
                    if (GoingToFull == true) {//选增长一般的rule
                        GUI_InterfaceRef.GetComponent<GUI_Interface>().ChangeRule(4);
                        currentRule = GUI_InterfaceRef.GetComponent<GUI_Interface>().ReturnChangeRule();
                    }
                    if (GoingToFull == true && averageVNdensity > 1.7) {//选VN小的，且增长一般的rule
                        GUI_InterfaceRef.GetComponent<GUI_Interface>().ChangeRule(1);
                        currentRule = GUI_InterfaceRef.GetComponent<GUI_Interface>().ReturnChangeRule();
                    }
                    if (GoingToDead == true){//选增长最大的，无论VN
                        GUI_InterfaceRef.GetComponent<GUI_Interface>().ChangeRule(7);
                        currentRule = GUI_InterfaceRef.GetComponent<GUI_Interface>().ReturnChangeRule();
                    }
                    if (GoingToDead == true && averageVNdensity > 1.5) {//选增长大的，且VN小的，----VN小的分数高，占70，，，layer大的分数高，占30
                        GUI_InterfaceRef.GetComponent<GUI_Interface>().ChangeRule(7);
                        currentRule = GUI_InterfaceRef.GetComponent<GUI_Interface>().ReturnChangeRule();
                    }
                }

                //if (currentVoxelObj.GetComponent<Voxel>().GetAge() > 3)
                //{
                //    currentRule = deathrule;
                //}

                //if (layerdensity > 0.2)
                //{
                //    currentRule = rule8;
                //}
                //if (layerdensity > 0.3&&averageVNdensity>2)
                //{
                //    currentRule = rule7;
                //}
                //if (layerdensity < 0.05)
                //{
                //    currentRule = rule3;
                //}
                //if (layerdensity < 0.1 && averageVNdensity > 1.5)
                //{
                //    currentRule = rule2;
                //}

                //..........

                //get the instructions
                int inst0 = currentRule.getInstruction(0);
                int inst1 = currentRule.getInstruction(1);
                int inst2 = currentRule.getInstruction(2);
                int inst3 = currentRule.getInstruction(3);

                // Rule Set 1: for voxels that are alive
                if (currentVoxelState == 1)
                {
                    // If there are less than two neighbours I am going to die
                    if (aliveNeighbours < inst0)
                    {
                        currentVoxelObj.GetComponent<Voxel>().SetFutureState(0);
                    }
                    // If there are two or three neighbours alive I am going to stay alive
                    if (aliveNeighbours >= inst0 && aliveNeighbours <= inst1)
                    {
                        currentVoxelObj.GetComponent<Voxel>().SetFutureState(1);
                    }
                    // If there are more than three neighbours I am going to die
                    if (aliveNeighbours > inst1)
                    {
                        currentVoxelObj.GetComponent<Voxel>().SetFutureState(0);
                    }
                }
                // Rule Set 2: for voxels that are death
                if (currentVoxelState == 0)
                {
                    // If there are exactly three alive neighbours I will become alive
                    if (aliveNeighbours >= inst2 && aliveNeighbours <= inst3)//if(aliveNeighbours >= inst2 && aliveNeighbours <= inst3)
                    {
                        currentVoxelObj.GetComponent<Voxel>().SetFutureState(1);
                    }
                }

                //age - here is an example of a condition where the cell is "killed" if its age is above a threshhold
                // in this case if this rule is put here after the Game of Life rules just above it, it would override 
                // the game of lie conditions if this condition was true

                if (currentVoxelObj.GetComponent<Voxel>().GetAge() > GUI_InterfaceRef.GetComponent<GUI_Interface>().GetAge())
                {
                    currentVoxelObj.GetComponent<Voxel>().SetFutureState(0);
                }


            }
        }
    }
   
    // Save the CA states - this is run after the future state of all cells is calculated to update/save
    //current state on the current level
    void SaveCA()
    {

        //counter stores the number of live cells on this level and is incremented below 
        //in the for loop for each cell with a state of 1
        totalAliveCells = 0;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                GameObject currentVoxelObj = voxelGrid[i, j, 0];
                int currentVoxelState = currentVoxelObj.GetComponent<Voxel>().GetState();
                // Save the voxel state
                GameObject savedVoxel = voxelGrid[i, j, currentFrame];

                savedVoxel.GetComponent<Voxel>().SetState(currentVoxelState);
                // Save the voxel age if voxel is alive
                if (currentVoxelState == 1)
                {

                    int currentVoxelAge = currentVoxelObj.GetComponent<Voxel>().GetAge();
                    savedVoxel.GetComponent<Voxel>().SetAge(currentVoxelAge);

                    totalAliveCells++;

                    //track oldest voxels
                    if (currentVoxelAge > maxAge)
                    {
                        maxAge = currentVoxelAge;
                    }
                }
            }
        }

        float totalcells = (length - 1) * (width - 1);
        layerdensity = totalAliveCells / totalcells;



        if (layerdensity > maxlayerdensity)
        {
            maxlayerdensity = layerdensity;
        }

        if (layerdensity < minlayerdensity)
        {
            minlayerdensity = layerdensity;
        }


        //this stores the density of live cells for each entire layer of cells(each level)
        layerDensities[currentFrame] = layerdensity;
        layerdensitySum += layerdensity;
        averageLayerdensity = 100 * layerdensitySum / currentFrame;

    }


    /// <summary>
    /// SETUP MOORES & VON NEUMANN 3D NEIGHBORS
    /// </summary>
    void SetupNeighbors3d()
    {
        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 1; j < length - 1; j++)
            {
                for (int k = 1; k < height - 1; k++)
                {
                    //the current voxel we are looking at...
                    GameObject currentVoxelObj = voxelGrid[i, j, k];

                    ////SETUP Von Neumann Neighborhood Cells////
                    Voxel[] tempNeighborsVN = new Voxel[6];

                    //left
                    Voxel VoxelLeft = voxelGrid[i - 1, j, k].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelLeft(VoxelLeft);
                    tempNeighborsVN[0] = VoxelLeft;

                    //right
                    Voxel VoxelRight = voxelGrid[i + 1, j, k].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelRight(VoxelRight);
                    tempNeighborsVN[2] = VoxelRight;

                    //back
                    Voxel VoxelBack = voxelGrid[i, j - 1, k].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelBack(VoxelBack);
                    tempNeighborsVN[3] = VoxelBack;

                    //front
                    Voxel VoxelFront = voxelGrid[i, j + 1, k].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelFront(VoxelFront);
                    tempNeighborsVN[1] = VoxelFront;

                    //below
                    Voxel VoxelBelow = voxelGrid[i, j, k - 1].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelBelow(VoxelBelow);
                    tempNeighborsVN[4] = VoxelBelow;

                    //above
                    Voxel VoxelAbove = voxelGrid[i, j, k + 1].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelAbove(VoxelAbove);
                    tempNeighborsVN[5] = VoxelAbove;

                    //Set the Von Neumann Neighbors [] in this Voxel
                    currentVoxelObj.GetComponent<Voxel>().setNeighbors3dVN(tempNeighborsVN);

                    ////SETUP Moore's Neighborhood////
                    Voxel[] tempNeighborsMO = new Voxel[26];

                    int tempcount = 0;
                    for (int m = -1; m < 2; m++)
                    {
                        for (int n = -1; n < 2; n++)
                        {
                            for (int p = -1; p < 2; p++)
                            {
                                if ((i + m >= 0) && (i + m < width) && (j + n >= 0) && (j + n < length) && (k + p >= 0) && (k + p < height))
                                {
                                    GameObject neighborVoxelObj = voxelGrid[i + m, j + n, k + p];
                                    if (neighborVoxelObj != currentVoxelObj)
                                    {
                                        Voxel neighborvoxel = voxelGrid[i + m, j + n, k + p].GetComponent<Voxel>();
                                        tempNeighborsMO[tempcount] = neighborvoxel;
                                        tempcount++;
                                    }
                                }
                            }
                        }
                    }
                    currentVoxelObj.GetComponent<Voxel>().setNeighbors3dMO(tempNeighborsMO);
                }
            }
        }
    }

 
    void AverageVNdensity()
    {
        float densitySum = 0.0f;
        float aliveSum = 0;

        foreach (var voxelObj in voxelGrid)
        {
            var voxel = voxelObj.GetComponent<Voxel>();

            if (voxel.GetState() == 1)
            {
                densitySum += voxel.GetDensity();
                aliveSum++;
                if (voxel.GetDensity() > maxVNdensity)
                {
                    maxVNdensity = voxel.GetDensity();
                }

                if (voxel.GetDensity() < minVNdensity)
                {
                    minVNdensity = voxel.GetDensity();
                }
            }
        }
        averageVNdensity = densitySum / aliveSum;
       
    }

    public float GetAverageLayerDensity()
    {
        return averageLayerdensity;
    }
    public float GetAverageVNDensity()
    {
        return averageVNdensity;
    }
    public float GetMaxLayerDensity()
    {
        return maxlayerdensity;
    }
    public float GetMinLayerDensity()
    {
        return minlayerdensity;
    }
    public float GetMaxVNDensity()
    {
        return maxVNdensity;
    }
    public float GetMinVNDensity()
    {
        return minVNdensity;
    }
    public float GetLayerDensity()
    {
        return layerdensity;
    }
    public float GetVisualModes()
    {
        return vizmode;
    }
    
    //void GetDensity()
    //{
    //    float densitySum = 0.0f;
    //    float aliveSum = 0;
    //    for (int i = 1; i < width - 1; i++)
    //    {
    //        for (int j = 1; j < length - 1; j++)
    //        {
    //            for (int k = 1; k < currentFrame; k++)
    //            {
    //                GameObject currentVoxelObj = voxelGrid[i, j, k];

    //                //UPDATE THE VON NEUMANN NEIGHBORHOOD DENSITIES FOR EACH VOXEL//
    //                Voxel[] tempNeighborsVN = currentVoxelObj.GetComponent<Voxel>().getNeighbors3dVN();

    //                foreach (Voxel vox in tempNeighborsVN)
    //                {
    //                    if (vox.GetState() == 1)
    //                    {
    //                       densitySum++;
    //                    }
    //                }
    //                int tempAlive = currentVoxelObj.GetComponent<Voxel>().GetState();

    //                    if (tempAlive == 1)
    //                    {
    //                    aliveSum++;
    //                    }
    //            }
    //        }
    //    }
    //    averageVNdensity = densitySum / aliveSum;
    //    Debug.Log("averageVNdensity are " + averageVNdensity);
    //}



    void updateDensities3d()
    {
        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 1; j < length - 1; j++)
            {
                for (int k = 1; k < currentFrame; k++)
                {
                    GameObject currentVoxelObj = voxelGrid[i, j, k];

                    //UPDATE THE VON NEUMANN NEIGHBORHOOD DENSITIES FOR EACH VOXEL//
                    Voxel[] tempNeighborsVN = currentVoxelObj.GetComponent<Voxel>().getNeighbors3dVN();
                    int alivecount = 0;
                    foreach (Voxel vox in tempNeighborsVN)
                    {
                        if (vox.GetState() == 1)
                        {
                            alivecount++;
                        }
                    }


                    currentVoxelObj.GetComponent<Voxel>().setDensity3dVN(alivecount);
                    if (alivecount > maxDensity3dVN)
                    {
                        maxDensity3dVN = alivecount;
                    }
                    if (alivecount < minDensity3dVN)
                    {
                        minDensity3dVN = alivecount;
                    }



                    //UPDATE THE MOORES NEIGHBORHOOD DENSITIES FOR EACH VOXEL//
                    Voxel[] tempNeighborsMO = currentVoxelObj.GetComponent<Voxel>().getNeighbors3dMO();
                    alivecount = 0;
                    foreach (Voxel vox in tempNeighborsMO)
                    {
                        if (vox.GetState() == 1)
                        {
                            alivecount++;
                        }
                    }
                    currentVoxelObj.GetComponent<Voxel>().setDensity3dMO(alivecount);
                    if (alivecount > maxDensity3dMO)
                    {
                        maxDensity3dMO = alivecount;
                    }
                    if (alivecount < minDensity3dMO)
                    {
                        minDensity3dMO = alivecount;
                    }


                }
            }
        }
    }

    void ExportPrepare()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    Voxel currentVoxel = voxelGrid[i, j, k].GetComponent<Voxel>();
                    if (currentVoxel.GetState() == 0)
                    {
                        Destroy(currentVoxel.gameObject);
                    }
                }
            }
        }
    }

    //private void CacheMaterials()
    //{
    //    _materials = new Material[_bodies.Length];

    //    for (int i = 0; i < _bodies.Length; i++)
    //    {
    //        var b = _bodies[i];
    //        if (b == null) continue;//???

    //        var m = _materials[i] = b.gameObject.GetComponent<MeshRenderer>().material;
    //        m.color = Spectrum[0];
    //    }
    //}

    //private void CreateJoints()
    //{
    //    _joints = new FixedJoint[_bodies.Length][];

    //    for (int i = 0; i < _bodies.Length; i++)
    //        _joints[i] = new FixedJoint[6];

    //    int countXY = CountX * CountY;
    //    int index = 0;

    //    for (int k = 0; k < CountZ; k++)
    //    {
    //        for (int j = 0; j < CountY; j++)
    //        {
    //            for (int i = 0; i < CountX; i++, index++)
    //            {
    //                var bodyA = _bodies[index];


    //                if (bodyA == null)
    //                    continue;

    //                var jointsA = _joints[index];

    //                // -x
    //                if (i > 0)
    //                {
    //                    var bodyB = _bodies[index - 1];

    //                    if (bodyB != null)
    //                        ConnectBodies(bodyA, jointsA, bodyB, _joints[index - 1], 0);
    //                }

    //                // -y
    //                if (j > 0)
    //                {
    //                    var bodyB = _bodies[index - CountX];

    //                    if (bodyB != null)
    //                        ConnectBodies(bodyA, jointsA, bodyB, _joints[index - CountX], 1);
    //                }

    //                // -z
    //                if (k > 0)
    //                {
    //                    var bodyB = _bodies[index - countXY];

    //                    if (bodyB != null)
    //                        ConnectBodies(bodyA, jointsA, bodyB, _joints[index - countXY], 2);
    //                }
    //            }
    //        }
    //    }
    //}

    //private void ConnectBodies(Rigidbody bodyA, FixedJoint[] jointsA, Rigidbody bodyB, FixedJoint[] jointsB, int index)
    //{
    //    var joint = bodyA.gameObject.AddComponent<FixedJoint>();
    //    joint.connectedBody = bodyB;

    //    joint.breakForce = BreakForce;
    //    joint.breakTorque = BreakTorque;

    //    jointsA[index] = jointsB[index + 3] = joint;
    //}

    //private void Fix(IEnumerable<int> selection)
    //{
    //    foreach (var b in _bodies.TakeSelection(selection))
    //    {
    //        if (b == null) continue;
    //        b.isKinematic = true;
    //        //SetMaterial(b.gameObject);
    //    }
    //}

    //IEnumerator UpdateBodyColors()
    //{
    //    const float factor = 0.75f;

    //    while (true)
    //    {
    //        for (int i = 0; i < _materials.Length; i++)
    //        {
    //            var m = _materials[i];

    //            if (m != null)
    //                m.color = Color.Lerp(m.color, GetTorqueColor(i), factor);
    //        }

    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    //private Color GetTorqueColor(int index)
    //{
    //    var joints = _joints[index];

    //    float sum = 0.0f;
    //    int count = 0;

    //    foreach (var j in _joints[index])
    //    {
    //        if (j != null)
    //        {
    //            sum += j.currentTorque.sqrMagnitude;
    //            count++;
    //        }
    //    }

    //    if (count == 0)
    //        return Spectrum[0];

    //    return Lerp(Spectrum, sum / (count * MaxTorque));
    //}


    //private Color GetForceColor(int index)
    //{
    //    var joints = _joints[index];

    //    float sum = 0.0f;
    //    int count = 0;

    //    foreach (var j in _joints[index])
    //    {
    //        if (j != null)
    //        {
    //            sum += j.currentForce.sqrMagnitude;
    //            count++;
    //        }
    //    }

    //    if (count == 0)
    //        return Spectrum[0];

    //    return Lerp(Spectrum, sum / (count * MaxTorque));
    //}

    //public static Color Lerp(IReadOnlyList<Color> colors, float factor)
    //{
    //    int last = colors.Count - 1;
    //    int i;
    //    factor = SlurMathf.Fract(factor * last, out i);

    //    if (i < 0)
    //        return colors[0];
    //    else if (i >= last)
    //        return colors[last];

    //    return Color.LerpUnclamped(colors[i], colors[i + 1], factor);
    //}


    //public void ResetGrid()
    //{
    //    ResetBodies();
    //    // ResetJoints();
    //}


    //private void ResetBodies()
    //{
    //    int index = 0;

    //    for (int k = 0; k < CountZ; k++)
    //    {
    //        for (int j = 0; j < CountY; j++)
    //        {
    //            for (int i = 0; i < CountX; i++, index++)
    //            {
    //                var body = _bodies[index];
    //                if (body == null) continue;

    //                // clear velocities
    //                body.velocity = new Vector3();
    //                body.angularVelocity = new Vector3();

    //                // reset position and orientation
    //                body.transform.localPosition = new Vector3(i, j, k);
    //                body.transform.localRotation = Quaternion.identity;
    //            }
    //        }
    //    }
    //}

    //private IEnumerable<int> LayerXY(int k)
    //{
    //    int offset = k * CountX * CountY;

    //    for (int j = 0; j < CountY; j++)
    //    {
    //        for (int i = 0; i < CountX; i++)
    //            yield return i + j * CountX + offset;
    //    }
    //}

    //private IEnumerable<int> LayerXZ(int j)
    //{
    //    int offset = j * CountX;
    //    int countXY = CountX * CountY;

    //    for (int k = 0; k < CountZ; k++)
    //    {
    //        for (int i = 0; i < CountX; i++)
    //            yield return i + offset + k * countXY;
    //    }
    //}

    //private IEnumerable<int> Block(int i0, int j0, int k0, int i1, int j1, int k1)
    //{
    //    int countXY = CountX * CountY;

    //    for (int k = k0; k < k1; k++)
    //    {
    //        for (int j = j0; j < j1; j++)
    //        {
    //            for (int i = i0; i < i1; i++)
    //                yield return i + j * CountX + k * countXY;
    //        }
    //    }
    //}


}
