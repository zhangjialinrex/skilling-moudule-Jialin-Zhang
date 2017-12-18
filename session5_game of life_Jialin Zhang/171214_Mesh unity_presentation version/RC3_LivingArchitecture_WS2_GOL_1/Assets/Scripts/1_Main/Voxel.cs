using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour {

    
    // VARIABLES
    //state
    private int state = 0;
    //next state
    private int futureState = 0;
    //age
    private int age = 0;
    //density3dMO
    private int density3dMO = 0;
    //density3dVN
    private int density3dVN = 0;
    //material property block for setting material properties with renderer
    private MaterialPropertyBlock props;
    //the mesh renderer
    private new MeshRenderer renderer;
    //var stores my 3d address
    public Vector3 address;
    
 

    //The Mesh Filter takes a mesh from your assets and passes it to the Mesh Renderer for rendering on the screen
    //One Voxel can contain different meshes which are the representation of different types of voxels
    public Mesh[] meshes;

    //variable to store a type for this voxel
    

    //von neumann neighbors
    private Voxel[] neighbors3dVN = new Voxel[6];

    //moore's neighbors
    private Voxel[] neighbors3dMO = new Voxel[26];

    private Voxel voxelAbove;
    private Voxel voxelBelow;
    private Voxel voxelRight;
    private Voxel voxelLeft;
    private Voxel voxelFront;
    private Voxel voxelBack;

    
    /// <summary>++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
   // public MeshTable meshtable;

    private MeshFilter _filter;
    // private int _currentState;//+++++
    private int _currentState;

    /// </summary>+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    void Start()
    {
        _filter = gameObject.GetComponent<MeshFilter>();

        //int vnDensity = getDensity3dVN();
        //if (changeMesh == 0)
        //{
        //    if (vnDensity > 4)
        //{
        //    _currentState = 0;
        //}
        //if (vnDensity <= 4 && vnDensity > 2)
        //{
        //    _currentState = 1;
        //}
        //if (vnDensity <= 2 && vnDensity > 0)
        //{
        //    _currentState = 2;
        //}
        //}
        //if (changeMesh == 1)
        //{
        //    if (vnDensity > 4)
        //    {
        //        _currentState = 3;
        //    }
        //    if (vnDensity <= 4 && vnDensity > 2)
        //    {
        //        _currentState = 4;
        //    }
        //    if (vnDensity <= 2 && vnDensity > 0)
        //    {
        //        _currentState = 5;
        //    }
        //}
        //if (changeMesh == 2)
        //{
        //    _currentState = 6;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        // do stuff

        //_filter.sharedMesh = MeshTable.Meshes[_currentState];
        
         int vnDensity = getDensity3dVN();
           
        
            if (vnDensity > 4)
            {
                _currentState = 0;
                
            }
            if (vnDensity <= 4 && vnDensity > 2)
            {
                _currentState = 1;
                
            }
            if (vnDensity <= 2 && vnDensity >= 0)
            {
                _currentState = 2;
                
            }
        
       

        _filter.sharedMesh = meshes[_currentState];

        


    }
    /// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
   

    public void SetupVoxel(int i, int j, int k, int _type)
    {
        //set reference to time end 
        props = new MaterialPropertyBlock();
        renderer = gameObject.GetComponent<MeshRenderer>();
        //initially set to false
        renderer.enabled = false;
        //set my address as a vector
        address = new Vector3 (i,j,k);
    }

    // Update function
    public void UpdateVoxel () {
        // Set the future state
        state = futureState;
        // If voxel is alive update age
        if (state == 1)
        {
            age++;
        }
        // If voxel is death disable the game object mesh renderer and set age to zero
        if (state == 0)
        {
            age = 0;
        }
        
    }



    /// <summary>
    /// Setters and Getters - Allow us to access and set private variables
    /// </summary>
    /// <param name="_state"></param>
    // Set the state of the voxel
    
    public void SetState(int _state){
        state = _state;
    }

    // Set the future state of the voxel
    public void SetFutureState(int _futureState){
        futureState = _futureState;
    }

    // Get the age of the voxel
    public void SetAge(int _age){
        age = _age;
    }

    // Get the state of the voxel
    public int GetState(){
        return state;
    }

    // Get the age of the voxel
    public int GetAge(){
        return age;
    }

    //Set 3d Moores Neighborhood Density 
    public void setDensity3dMO(int _density3dMO)
    {
        density3dMO = _density3dMO;
    }
    //Get 3d Moores Neighborhood Density 
    public int getDensity3dMO()
    {
        return density3dMO;
    }

    //Set 3d Von Neumann Neighborhood Density 
    public void setDensity3dVN(int _density3dVN)
    {
        density3dVN = _density3dVN;
    }
    //Get 3d Von Neumann Neighborhood Density 
    public int getDensity3dVN()
    {
        return density3dVN;
    }



    /// <summary>
    /// VOXEL NEIGHBORHOOD GETTERS/SETTERS
    /// </summary>
    /// 

    //MOORES NEIGHBORS (26 PER VOXEL)
    public void setNeighbors3dMO(Voxel[] _setNeighbors3dMO)
    {
        neighbors3dMO = _setNeighbors3dMO;
    }

    public Voxel[] getNeighbors3dMO()
    {
        return neighbors3dMO;
    }

    //VON NEUMANN NEIGHBORS (6 PER VOXEL)
    public void setNeighbors3dVN(Voxel[] _setNeighbors3dVN)
    {
        neighbors3dVN = _setNeighbors3dVN;
    }

    public Voxel[] getNeighbors3dVN()
    {
        return neighbors3dVN;
    }


    //voxel above this
    public void setVoxelAbove(Voxel _voxelAbove)
    {
        voxelAbove = _voxelAbove;
    }

    public Voxel getVoxelAbove()
    {
        return voxelAbove;
    }

    //voxel below this
    public void setVoxelBelow(Voxel _voxelBelow)
    {
        voxelBelow = _voxelBelow;
    }

    public Voxel getVoxelBelow()
    {
        return voxelBelow;
    }

    //voxel right of this
    public void setVoxelRight(Voxel _voxelRight)
    {
        voxelRight = _voxelRight;
    }

    public Voxel getVoxelRight()
    {
        return voxelRight;
    }

    //voxel left of this
    public void setVoxelLeft(Voxel _voxelLeft)
    {
        voxelLeft = _voxelLeft;
    }

    public Voxel getVoxelLeft()
    {
        return voxelLeft;
    }

    //voxel in front of this
    public void setVoxelFront(Voxel _voxelFront)
    {
        voxelFront = _voxelFront;
    }

    public Voxel getVoxelFront()
    {
        return voxelFront;
    }

    //voxel in back of this
    public void setVoxelBack(Voxel _voxelBack)
    {
        voxelBack = _voxelBack;
    }

    public Voxel getVoxelBack()
    {
        return voxelBack;
    }


    public float GetDensity()
    {
        int sum = 0;


        //sum += voxelAbove.GetState();
        //sum += voxelBelow.GetState();
        //sum += voxelRight.GetState();
        //sum += voxelLeft.GetState();
        //sum += voxelFront.GetState();
        //sum += voxelBack.GetState();


        // foreach (var v in GetNeighbors())
        //     sum += v.GetState();

        //return sum / 6.0f;
        //foreach (var v in density3dVN())
        //{
        //    sum += v.GetState();
        //}
        
        sum=getDensity3dVN();
        return sum;
    }

    //返回具体哪个的值
    //public IEnumerable<Voxel> GetNeighbors()
    //{
    //    yield return voxelBelow;
    //    yield return voxelAbove;
    //    yield return voxelLeft;
    //    yield return voxelRight;
    //    yield return voxelFront;
    //    yield return voxelBack;
    //}

    // Update the voxel display
    public void VoxelDisplay()
    {
        if (state == 1)
        {
            Color col = new Color(1, 1, 1, 1);
            
            // Set Color
            
            props.SetColor("_Color", col);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }

        if (state == 0)
        {
            renderer.enabled = false;
        }
    }

    public void VoxelDisplay(int _r, int _g, int _b)
    {
        if (state == 1)
        {
            // Set Color
            Color col = new Color(_r, _g, _b, 1);
            props.SetColor("_Color", col);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }

        if (state == 0)
        {
            renderer.enabled = false;
        }
    }

    
        public void VoxelDisplayEachColor()
    {
        if (state == 1)
        {
            if (_currentState == 0)
            {
                Color color1 = new Color(0.15f, 0.8f, 0.9f, 1);//vn low --blue
                props.SetColor("_Color", color1);
            }
            if (_currentState == 1)
            {
                Color color2 = new Color(1, 1, 1, 1);//vn middle --white
                props.SetColor("_Color", color2);

            }
            if (_currentState == 2)
            {
                Color color3 = new Color(1f, 0.43f, 0.5f);//vn high --red
                props.SetColor("_Color", color3);

            }
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }

        if (state == 0)
        {
            renderer.enabled = false;
        }
    }
    /// <summary>
    /// Create Color Gradient Between 2 Colors by Age
    /// </summary>
    /// <param name="_maxAge"></param>
    public void VoxelDisplayAge(int _maxAge)
    {
        if (state == 1)
        {
            // Remap the age value relative to maxage to range of 0,1
            float mappedvalue = Remap(age, 0, _maxAge, 0.0f, 1.0f);
            //two colors to interpolate between
            Color color1 = new Color(1, 1, 1, 1);
            Color color2 = new Color(1, 1, 0, 1);
            //interpolate color from mapped value
            Color mappedcolor = Color.Lerp(color1, color2, mappedvalue);
            props.SetColor("_Color", mappedcolor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }
        if (state == 0)
        {
            renderer.enabled = false;
        }
    }

    /// <summary>
    /// Create Color Gradient Between 2 Colors by Density
    /// </summary>
    /// <param name="_maxdensity3dMO"></param>
    public void VoxelDisplayLayerDensity(float _layerdensity, float _minlayerdensity,float _maxlayerdensity)
    {
        if (state == 1)
        {
            // Remap the density value relative to maxdensity to range of 0,1
            float mappedvalue = Remap(_layerdensity, 0, _maxlayerdensity, 0.0f, 1.0f);
            //two colors to interpolate between
            Color color1 = new Color(0.75f, 0.9f, 1f, 1);
            Color color2 = new Color(1f, 0.43f, 0.5f, 1);///(0.447f, 0.803f, 0.980f, 1)
            //interpolate color from mapped value
            Color mappedcolor = Color.Lerp(color1, color2, mappedvalue);
            props.SetColor("_Color", mappedcolor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }
        if (state == 0)
        {
            renderer.enabled = false;
        }
    }

    /// <summary>
    /// Create Color Gradient Between 2 Colors by Density
    /// </summary>
    /// <param name="_maxdensity3dMO"></param>
    public void VoxelDisplayDensity3dMO(int _mindensity3dMO, int _maxdensity3dMO)
    {
        if (state == 1)
        {
            // Remap the density value relative to maxdensity to range of 0,1
            float mappedvalue = Remap(density3dMO, 0, _maxdensity3dMO, 0.0f, 1.0f);
           
            //two colors to interpolate between
            Color color1 = new Color(1f, 1f, 1f, 1);
            Color color2 = new Color(1f, 0.5f, 0.5f, 1);
            //interpolate color from mapped value
            Color mappedcolor = Color.Lerp(color1, color2, mappedvalue);
            props.SetColor("_Color", mappedcolor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }
        if (state == 0)
        {
            renderer.enabled = false;
        }
    }


    /// <summary>
    /// Create Color Gradient Between 2 Colors by Density
    /// </summary>
    /// <param name="_maxdensity3dMO"></param>
    public void VoxelDisplayDensity3dVN( int _mindensity3dVN, int _maxdensity3dVN)
    {
        if (state == 1)
        {
            // Remap the density value relative to maxdensity to range of 0,1
            float mappedvalue = Remap(density3dVN, _mindensity3dVN, _maxdensity3dVN, 0.0f, 1.0f);
            //two colors to interpolate between
            Color color1 = new Color (1f, 1f, 1f, 1);
            Color color2 = new Color(0f, 0.7f, 1f, 1);
            //interpolate color from mapped value
            Color mappedcolor = Color.Lerp(color1, color2, mappedvalue);
            props.SetColor("_Color", mappedcolor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }
        if (state == 0)
        {
            renderer.enabled = false;
        }
    }


    // Remap numbers - used here for getting a gradient of color across a range
    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
