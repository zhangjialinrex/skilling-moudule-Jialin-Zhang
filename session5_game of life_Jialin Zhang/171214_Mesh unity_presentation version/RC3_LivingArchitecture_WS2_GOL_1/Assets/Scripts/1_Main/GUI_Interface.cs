using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SpatialSlur.SlurCore;
using SpatialSlur.SlurField;
using SpatialSlur.SlurUnity;

public class GUI_Interface : MonoBehaviour
{

    public GameObject voxelGrid;
    public GameObject voxelPrefab;


    List<string> ruleNames = new List<string>() { "Selet rule", "Rule1", "Rule2", "Rule3", "Rule4", "Rule5", "Rule6", "Rule7", "Rule8" };
    List<string> seedImageNames = new List<string>() { "Selet seedImage", "20*20 one leg", "40*40 four Legs", "40*40 four Legs", "40*40 three Legs",
        "40*40 two Legs", "40*40 one Legs"}; //, "SeedImage7", "SeedImage8", "SeedImage9", "SeedImage10" , "SeedImage11" , "SeedImage12" , "SeedImage13" , "SeedImage14" , "SeedImage15"
    List<string> VisualModeNames = new List<string>() {  "LayerDensity", "Mesh Types", "VN Density", "MO Density", "Age Range"};
    
    public Dropdown dropdown_rule;
    public Dropdown dropdown_seedImage;
   
    public RawImage rawSeedImage;
    public Slider swiftTimeEnd;
    public Slider swiftAge;
    public Text SetSeedImageNameText;
    public Text sliderText;
    public Text sliderText_age;
    public Text ruleText;
    public Text aliveRatioText;
    public Text VNdensityText;
    public Text MaxLayerdensityText;
    public Text MinLayerdensityText;
    public Text MaxVNdensityText;
    public Text MinVNdensityText;
    public Text layerDensity;
    public Text UsingRuleText;
    public Text VisualModeText;
    public Texture2D[] SeedImage;

    public Camera Main;
    public Camera Top;
    public Camera Left;



    Texture2D seedImage;
    GOLRule _usingRule;

    GOLRule rule1 = new GOLRule();
    GOLRule rule2 = new GOLRule();
    GOLRule rule3 = new GOLRule();
    GOLRule rule4 = new GOLRule();
    GOLRule rule5 = new GOLRule();
    GOLRule rule6 = new GOLRule();
    GOLRule rule7 = new GOLRule();
    GOLRule rule8 = new GOLRule();
    GOLRule whichRule = new GOLRule();
    GOLRule[] ruleStore = new GOLRule[9];
    int width;
    int length;
    int realTimeEnd;
    int realAge;
    float floatTimeEnd;
    float floatAge;
    int viewusingRule;
    int displayMode;
    float timeInGUI=0.5f;
    float swiftMode = 0;
    int cirle=0;
    //int changeDisplayMode = 0;

    bool selectedSeedImage = false;

    bool selectedRule = false;

    bool startedGame = false;

     bool stoppedGame = false;

    bool zoomIn = false;

    //void GetStuffFromEnv()
    //{
    //    int timeEndRef = voxelGrid.GetComponent<Environment>().timeEnd;
    //}



    void PopulateDropdownList()
    {
        dropdown_rule.AddOptions(ruleNames);
        dropdown_seedImage.AddOptions(seedImageNames);
        
    }
    public void SeedImageSelection()
    {
        int index = dropdown_seedImage.value;

        if (index == 0)
        {
            //seedImage = SeedImage[0];
            selectedSeedImage = false;
        }
        if (index > 0)
        {
            seedImage = SeedImage[index];
            rawSeedImage.GetComponent<RawImage>().texture = seedImage;
            width = seedImage.width;
            length = seedImage.height;
            selectedSeedImage = true;
        }
        
    }
  
  
    public int SeedImageWidth()
    {
        return width;
    }
    public int SeedImageLength()
    {
        return length;
    }
    public bool GetSelectedSeedImage()
    {

        return selectedSeedImage;

    }
    public Texture2D GetseedImage()
    {
        return seedImage;
    }

    public void RuleSelection()
    {
        //ruleSetting();

        int index2 = dropdown_rule.value;
        if (index2 == 0)
        {
           // _usingRule= ruleStore[0];
            selectedRule = false;
        }
        if (index2 > 0)
        {

            _usingRule = ruleStore[index2];
            selectedRule = true;
            ruleText.GetComponent<Text>().text = _usingRule.getInstruction(0).ToString() + "," + _usingRule.getInstruction(1).ToString() 
                + "," + _usingRule.getInstruction(2).ToString() + "," + _usingRule.getInstruction(3).ToString();
        }
    }
    public GOLRule GetRule()
    {
        return _usingRule;
    }

    public void ChangeRule(int _whichRule) {
         whichRule=ruleStore[_whichRule];
        viewusingRule = _whichRule;
            }

    public GOLRule ReturnChangeRule() {
        return whichRule;
    }

    public bool GetSelectedRule()
    {

        return selectedRule;

    }
    public void StartGame()
    {
        RuleSelection();
        SeedImageSelection();
       
        startedGame = true;
        stoppedGame = false;
       
        SetTimeEnd();
    }
    public bool GetStartGame()
    {
       
        return startedGame;

    }
    public void StopGame()
    {  
        startedGame = false;
        stoppedGame = true;
        SetTimeEnd();
        //selectedSeedImage =false;
        //selectedRule = false;
    }
    public bool GetStopGame()
    {
        return stoppedGame;
    }

    public void SetTimeEnd() {
        floatTimeEnd=swiftTimeEnd.value;
        realTimeEnd = (int)floatTimeEnd;
    }
    public int GetTimeEnd() {
        return realTimeEnd;
    }
    public void SetAge()
    {
        floatAge = swiftAge.value;
        realAge = (int)floatAge;
    }
    public int GetAge()
    {
        return realAge;
    }

    public void QuickGame()
    {
        _usingRule = ruleStore[2];
        
        seedImage = SeedImage[1];
       
        rawSeedImage.GetComponent<RawImage>().texture = seedImage;
        width = seedImage.width;
        length = seedImage.height;
        ResetCamera();
        selectedSeedImage = true;
        selectedRule = true;
        
        SetTimeEnd();
        startedGame = true;
        stoppedGame = false;
        
        
        
    }
    public void DisplayMode()
    {
        displayMode++;

        if (displayMode == 0)
        {
            //Left.GetComponent<Camera>().enabled = true;
            //Top.GetComponent<Camera>().enabled = true;
        }
        else { 
            //Left.GetComponent<Camera>().enabled = false;
            //Top.GetComponent<Camera>().enabled = false;
            zoomIn = true;

        }

        if (displayMode>=2) {
            //Left.GetComponent<Camera>().enabled = true;
            //Top.GetComponent<Camera>().enabled = true;
            displayMode = 0;
            zoomIn = false;
            
        }
        
    }

    void ResetCamera() {
        if (width == 20)
        {
            if (startedGame == false)
            {
                Top.GetComponent<Transform>().position = new Vector3(6.7f, 100, 10.6f);
                Top.GetComponent<Camera>().orthographicSize = 16.5f;
                Left.GetComponent<Transform>().position = new Vector3(7.7f, 33.1f, 10f);
                Main.GetComponent<Transform>().position = new Vector3(27.5f, 53.4f, -15f);
                Main.GetComponent<Camera>().orthographicSize = 35f;
            }
        }
        if (width == 40)
        {
            if (startedGame == false)
            {
                Top.GetComponent<Transform>().position = new Vector3(16.6f, 100, 20.16f);
                Top.GetComponent<Camera>().orthographicSize = 26.1f;
                Left.GetComponent<Transform>().position = new Vector3(17.5f, 33.1f, 10f);
                Main.GetComponent<Transform>().position = new Vector3(34.7f, 58.9f, -15f);
                Main.GetComponent<Camera>().orthographicSize = 42.5f;
            }
        }

    }
        // Use this for initialization
        void Start () {
        
       
        PopulateDropdownList();
        rule1.setupRule(2, 3, 2, 2);
        rule2.setupRule(2, 3, 3, 3);
        rule3.setupRule(2, 6, 4, 7);
        rule4.setupRule(3, 4, 3, 5);
        rule5.setupRule(4, 5, 3, 4);
        rule6.setupRule(4, 5, 3, 6);
        rule7.setupRule(6, 7, 2, 7);
        rule8.setupRule(6, 7, 2, 2);
        //deathrule.setupRule(0, 0, 0, 0);

        ruleStore[0] = rule1;
        ruleStore[1] = rule1;
        ruleStore[2] = rule2;
        ruleStore[3] = rule3;
        ruleStore[4] = rule4;
        ruleStore[5] = rule5;
        ruleStore[6] = rule6;
        ruleStore[7] = rule7;
        ruleStore[8] = rule8;

        

    }
    void Update()
    {
        SetAge();
        SetTimeEnd();
        
        RuleSelection();
        SeedImageSelection();
        ResetCamera();
        if (zoomIn == true)
        {

            timeInGUI = 0.5f;
            
            swiftMode += timeInGUI;
            if (Main.GetComponent<Camera>().orthographicSize > 15)
            {
                Main.GetComponent<Camera>().orthographicSize -= timeInGUI;
            }
            if (Main.GetComponent<Camera>().orthographicSize <= 15&&Main.GetComponent<Camera>().orthographicSize> 9 && cirle<=1)
            {
                Main.GetComponent<Camera>().orthographicSize -= 0.25f;
            }
          
           

            voxelGrid.GetComponent<Environment>().AutoChangeMode(swiftMode);
            if (swiftMode > 40f)
            {
                swiftMode = 0;
                cirle++;
                
            }
            
            if (Main.GetComponent<Camera>().orthographicSize < 15 && Main.GetComponent<Camera>().orthographicSize >= 9 && cirle > 1)
            {
                Main.GetComponent<Camera>().orthographicSize += 0.25f;
                
                //Debug.Log(cirle);
                //Debug.Log(Main.GetComponent<Camera>().orthographicSize);
            }
            if (cirle > 4)
            {
                cirle = 0;
            }

        }
        if (zoomIn == false)
        {
            if (Main.GetComponent<Camera>().orthographicSize < 35 && timeInGUI == 0.5f)
            {
                Main.GetComponent<Camera>().orthographicSize += timeInGUI;
            }
            else {
                timeInGUI += 0.1f;
            }
        }


        VisualModeText.GetComponent<Text>().text = VisualModeNames[(int)voxelGrid.GetComponent<Environment>().GetVisualModes()];
        UsingRuleText.GetComponent<Text>().text = ruleNames[viewusingRule];
        SetSeedImageNameText.GetComponent<Text>().text = seedImageNames[dropdown_seedImage.value];
        sliderText.GetComponent<Text>().text = realTimeEnd.ToString();
        sliderText_age.GetComponent<Text>().text = realAge.ToString();

        float aliveRatioNum = voxelGrid.GetComponent<Environment>().GetAverageLayerDensity() ;
        string aliveRatioStri = string.Format("{0:f2}", aliveRatioNum);
        aliveRatioText.GetComponent<Text>().text = aliveRatioStri+"%";

        float VNdensityNum = voxelGrid.GetComponent<Environment>().GetAverageVNDensity();
        string VNdensityStri = string.Format("{0:f4}", VNdensityNum);
        VNdensityText.GetComponent<Text>().text = VNdensityStri ;

        float MaxLayerdensityNum = voxelGrid.GetComponent<Environment>().GetMaxLayerDensity();
        string MaxLayerdensityStri = string.Format("{0:f2}", MaxLayerdensityNum*100);
        MaxLayerdensityText.GetComponent<Text>().text = MaxLayerdensityStri + "%";

        float MinLayerdensityNum = voxelGrid.GetComponent<Environment>().GetMinLayerDensity();
        string MinLayerdensityStri = string.Format("{0:f2}", MinLayerdensityNum * 100);
        MinLayerdensityText.GetComponent<Text>().text = MinLayerdensityStri + "%";

        float MaxVNdensityNum = voxelGrid.GetComponent<Environment>().GetMaxVNDensity();
        string MaxVNdensityStri = string.Format("{0:f4}", MaxVNdensityNum);
        MaxVNdensityText.GetComponent<Text>().text = MaxVNdensityStri;

        float MinVNdensityNum = voxelGrid.GetComponent<Environment>().GetMinVNDensity();
        string MinVNdensityStri = string.Format("{0:f4}", MinVNdensityNum);
        MinVNdensityText.GetComponent<Text>().text = MinVNdensityStri;

        float LayerdensityNum = voxelGrid.GetComponent<Environment>().GetLayerDensity();
        string LayerdensityStri = string.Format("{0:f2}", LayerdensityNum * 100);
        layerDensity.GetComponent<Text>().text = LayerdensityStri + "%";
        

    }

}
