using System;
using System.IO;
using uLink;
using UnityEngine;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace WorldEditor
{
    public class Editor : MonoBehaviour
    {
        public bool ShowList = false;
        public GameObject TempGameObject;
        public LoadingHandler.LoadObjectFromBundle SpawnedObject;
        public int Grid = 0;
        public static GUIStyle style2 = new GUIStyle();
        private float flySpeed = 3f;

        GUIContent[] comboBoxList;
        private ComboBox comboBoxControl;
        private GUIStyle listStyle = new GUIStyle();
        public bool _fly = false;
        public PlayerClient localPlayerClient;
        public Controllable controllable;
        public Character localCharacter;
        public HumanController localController;

        public void Start()
        {
            style2 = new GUIStyle();

            style2.fontSize = Screen.height * 2 / 110;
            style2.richText = true;
            style2.alignment = TextAnchor.MiddleCenter;
            style2.normal.textColor = Color.yellow;

            comboBoxList = new GUIContent[WorldEditor.Instance.Prefabs.Count];
            for (int i = 0; i < comboBoxList.Length; i++)
            {
                comboBoxList[i] = new GUIContent(WorldEditor.Instance.Prefabs[i]);
            }

            listStyle.normal.textColor = Color.white;
            listStyle.onHover.background =
                listStyle.hover.background = new Texture2D(2, 2);
            listStyle.padding.left =
                listStyle.padding.right =
                    listStyle.padding.top =
                        listStyle.padding.bottom = 4;

            comboBoxControl = new ComboBox(new Rect(Screen.width / 2, Screen.height - Screen.height + 25, 200, 30),
                comboBoxList[0], comboBoxList, "button", "box", listStyle);
        }

        public void Update()
        {
            if (_fly)
            {
                CCMotor ccmotor = this.localController.ccmotor;
                var defaultMovement = new CCMotor.Movement?(ccmotor.movement.setup);

                ccmotor.velocity = Vector3.zero;
                Vector3 forward = this.localCharacter.eyesAngles.forward;
                Vector3 right = this.localCharacter.eyesAngles.right;
                if (!ChatUI.IsVisible())
                {
                    if (Input.GetKey(KeyCode.W))
                    {
                        ccmotor.velocity += forward * (ccmotor.movement.setup.maxForwardSpeed * flySpeed);
                    }

                    if (Input.GetKey(KeyCode.S))
                    {
                        ccmotor.velocity -= forward * (ccmotor.movement.setup.maxBackwardsSpeed * flySpeed);
                    }

                    if (Input.GetKey(KeyCode.A))
                    {
                        ccmotor.velocity -= right * (ccmotor.movement.setup.maxSidewaysSpeed * flySpeed);
                    }

                    if (Input.GetKey(KeyCode.D))
                    {
                        ccmotor.velocity += right * (ccmotor.movement.setup.maxSidewaysSpeed * flySpeed);
                    }

                    if (Input.GetKey(KeyCode.Space))
                    {
                        ccmotor.velocity += Vector3.up * (defaultMovement.Value.maxAirAcceleration * flySpeed);
                    }
                }

                if (ccmotor.velocity == Vector3.zero)
                {
                    ccmotor.velocity += Vector3.up * (ccmotor.settings.gravity * Time.deltaTime * 0.5f);
                }
            }

            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                flySpeed += 0.5f;
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    flySpeed += 1.5f;
            }
            else if (Input.GetKeyDown(KeyCode.PageDown))
            {
                flySpeed -= 0.5f;
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    flySpeed -= 1.5f;
            }
            
            if (!WorldEditor.Instance.Enabled)
            {
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                if (ShowList)
                {
                    Screen.lockCursor = true;
                    ShowList = false;
                }
                else
                {
                    Screen.lockCursor = false;
                    ShowList = true;
                }
            }

            if (SpawnedObject != null && SpawnedObject.ObjectInstantiate != null)
            {
                if (Input.GetKey(KeyCode.Keypad1))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position += new Vector3(0.1f, 0, 0); //POSX
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position += new Vector3(1f, 0, 0); //POSX
                    }
                    else
                    {
                        SpawnedObject.ObjectInstantiate.transform.position += new Vector3(0.01f, 0, 0); //POSX
                    }
                }

                if (Input.GetKey(KeyCode.Keypad2))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position -= new Vector3(0.1f, 0, 0); //POSX
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position -= new Vector3(1f, 0, 0); //POSX
                    }
                    else
                    {
                        SpawnedObject.ObjectInstantiate.transform.position -= new Vector3(0.01f, 0, 0); //POSX
                    }
                }

                if (Input.GetKey(KeyCode.Keypad4))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position += new Vector3(0, 0, 0.1f); //POSZ
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position += new Vector3(0, 0, 1f); //POSZ
                    }
                    else
                    {
                        SpawnedObject.ObjectInstantiate.transform.position += new Vector3(0, 0, 0.01f); //POSZ
                    }
                }

                if (Input.GetKey(KeyCode.Keypad5))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position -= new Vector3(0, 0, 0.1f); //POSZ
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position -= new Vector3(0, 0, 1f); //POSZ
                    }
                    else
                    {
                        SpawnedObject.ObjectInstantiate.transform.position -= new Vector3(0, 0, 0.01f); //POSZ
                    }
                }

                if (Input.GetKey(KeyCode.Keypad7))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position += new Vector3(0, 0.1f, 0); //POSY
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position += new Vector3(0, 1f, 0); //POSY
                    }
                    else
                    {
                        SpawnedObject.ObjectInstantiate.transform.position += new Vector3(0, 0.01f, 0); //POSY
                    }
                }

                if (Input.GetKey(KeyCode.Keypad8))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position -= new Vector3(0, 0.1f, 0); //POSY
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position -= new Vector3(0, 1f, 0); //POSY
                    }
                    else
                    {
                        SpawnedObject.ObjectInstantiate.transform.position -= new Vector3(0, 0.01f, 0); //POSY
                    }
                }

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.x = rot.x + 0.1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.x = rot.x + 1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.x = rot.x + 0.01f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                }

                if (Input.GetKey(KeyCode.DownArrow))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.x = rot.x - 0.1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.x = rot.x - 1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.x = rot.x - 0.01f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.y = rot.y + 0.1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.y = rot.y + 1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.y = rot.y + 0.01f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                }

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.y = rot.y - 0.1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.y = rot.y - 1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.y = rot.y - 0.01f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                }

                if (Input.GetKey(KeyCode.Keypad3))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.z = rot.z + 0.1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.z = rot.z + 1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.z = rot.z + 0.01f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                }

                if (Input.GetKey(KeyCode.KeypadEnter))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.z = rot.z - 0.1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.z = rot.z - 1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.z = rot.z - 0.01f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                }


                if (Input.GetKey(KeyCode.KeypadMultiply))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        SpawnedObject.ObjectInstantiate.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f); //SIZE +
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        SpawnedObject.ObjectInstantiate.transform.localScale += new Vector3(1f, 1f, 1f);
                    }
                    else
                    {
                        SpawnedObject.ObjectInstantiate.transform.localScale +=
                            new Vector3(0.01f, 0.01f, 0.01f); //SIZE +
                    }
                }

                if (Input.GetKey(KeyCode.KeypadMinus))
                {
                    if (SpawnedObject.ObjectInstantiate.transform.localScale == Vector3.zero)
                    {
                        return;
                    }

                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        SpawnedObject.ObjectInstantiate.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f); //SIZE -
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        SpawnedObject.ObjectInstantiate.transform.localScale -= new Vector3(1f, 1f, 1f);
                    }
                    else
                    {
                        SpawnedObject.ObjectInstantiate.transform.localScale -=
                            new Vector3(0.01f, 0.01f, 0.01f); //SIZE -
                    }
                }
            }
        }

        public void OnGUI()
        {
            if (!WorldEditor.Instance.Enabled)
            {
                return;
            }

            try
            {
                Vector3 playerloc = Camera.main.ViewportToWorldPoint(transform.localPosition);


                if (SpawnedObject != null && SpawnedObject.ObjectInstantiate != null)
                {
                    Collider collider = SpawnedObject.ObjectInstantiate.collider;
                    bool hascollider = false;
                    if (collider != null)
                    {
                        hascollider = SpawnedObject.ObjectInstantiate
                            .collider.enabled;
                    }

                    GUI.Label(new Rect(0, Screen.height - 100, Screen.width, 20), "POS:"
                                                                                  + SpawnedObject.ObjectInstantiate
                                                                                      .transform.position.ToString()
                                                                                  + " ROT:" + SpawnedObject
                                                                                      .ObjectInstantiate.transform
                                                                                      .rotation.ToString()
                                                                                  + " Size:" +
                                                                                  SpawnedObject.ObjectInstantiate
                                                                                      .transform.localScale.ToString()
                                                                                  + " Col: " +
                                                                                  hascollider, style2);
                }

                string prefab = "";
                if (ShowList)
                {
                    int selectedItemIndex = comboBoxControl.Show();
                    if (comboBoxList[selectedItemIndex] != null)
                    {
                        prefab = comboBoxList[selectedItemIndex].text;
                    }
                }

                GUI.Box(new Rect(0, 60, 140, 90), "Object Spawn");
                GUI.Label(new Rect(Screen.width / 2, Screen.height - Screen.height + 10, 200, 30),
                    string.Format("<b><color=#298A08>" + prefab + "</color></b> "));

                if (GUI.Button(new Rect(0, 0, 120, 20), "Fly (" + flySpeed + ")"))
                {
                    _fly = !_fly;
                    if (_fly)
                    {
                        this.localPlayerClient = PlayerClient.GetLocalPlayer();
                        this.controllable = this.localPlayerClient.controllable;
                        this.localCharacter = this.controllable.character;
                        this.localController = this.localCharacter.controller as HumanController;
                    }
                }

                if (GUI.Button(new Rect(10, 80, 120, 20), "Spawn"))
                {
                    try
                    {
                        TempGameObject = new GameObject();
                        SpawnedObject = TempGameObject.AddComponent<LoadingHandler.LoadObjectFromBundle>();
                        SpawnedObject.Create(prefab, WorldEditor.Instance.PrefabBundleDictionary[prefab],
                            new Vector3(playerloc.x + 10f, playerloc.y, playerloc.z + 10f),
                            new Quaternion(0f, 0f, 0f, 0f),
                            new Vector3(1f, 1f, 1f));
                        UnityEngine.Object.DontDestroyOnLoad(TempGameObject);
                        Screen.lockCursor = true;
                    }
                    catch
                    {
                        Rust.Notice.Inventory("", "Failed to create prefab!");
                        // ignore
                    }
                }

                if (GUI.Button(new Rect(10, 100, 120, 20), "Destroy"))
                {
                    if (SpawnedObject != null && SpawnedObject.ObjectInstantiate != null)
                    {
                        WorldEditor.Instance.AllSpawnedObjects.Remove(SpawnedObject);
                        UnityEngine.Object.Destroy(SpawnedObject.ObjectInstantiate);
                        SpawnedObject = null;
                        UnityEngine.Object.Destroy(TempGameObject);
                        TempGameObject = null;
                    }
                }

                if (GUI.Button(new Rect(10, 120, 120, 20), "Player loc"))
                {
                    var b = File.Exists(RustBuster2016.API.Hooks.GameDirectory +
                                        "\\RB_Data\\WorldEditor\\PlayerLocation.txt");
                    string text = "";

                    if (!b)
                        File.Create(RustBuster2016.API.Hooks.GameDirectory +
                                    "\\RB_Data\\WorldEditor\\PlayerLocation.txt");
                    else
                        text = File.ReadAllText(RustBuster2016.API.Hooks.GameDirectory +
                                                "\\RB_Data\\WorldEditor\\PlayerLocation.txt");
                    text += "X: " + playerloc.x + ", Y: " + playerloc.y + ", Z: " + playerloc.z + "\r\n";
                    File.WriteAllText(
                        RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor\\PlayerLocation.txt", text);
                }

                GUI.Box(new Rect(0, 170, 140, 120), "File Manager");
                if (GUI.Button(new Rect(10, 190, 120, 20), "SAVE CURRENT"))
                {
                    if (SpawnedObject != null && SpawnedObject.ObjectInstantiate != null)
                    {
                        string line = "" + WorldEditor.Instance.PrefabBundleDictionary[SpawnedObject.Name] + ":" +
                                      SpawnedObject.Name + ":" +
                                      SpawnedObject.ObjectInstantiate.transform.position.x.ToString() + "," +
                                      SpawnedObject.ObjectInstantiate.transform.position.y.ToString() + "," +
                                      SpawnedObject.ObjectInstantiate.transform.position.z.ToString() + ":" +
                                      SpawnedObject.ObjectInstantiate.transform.rotation.x.ToString() + "," +
                                      SpawnedObject.ObjectInstantiate.transform.rotation.y.ToString() + "," +
                                      SpawnedObject.ObjectInstantiate.transform.rotation.z.ToString() + "," +
                                      SpawnedObject.ObjectInstantiate.transform.rotation.w.ToString() + ":" +
                                      SpawnedObject.ObjectInstantiate.transform.localScale.x.ToString() + "," +
                                      SpawnedObject.ObjectInstantiate.transform.localScale.y.ToString() + "," +
                                      SpawnedObject.ObjectInstantiate.transform.localScale.z.ToString();

                        var file = new System.IO.StreamWriter(WorldEditor.Instance.SavedObjectsPath, true);
                        file.WriteLine(line);
                        file.Close();
                    }
                }

                if (GUI.Button(new Rect(10, 210, 120, 20), "SAVEALL"))
                {
                    foreach (var x in WorldEditor.Instance.AllSpawnedObjects)
                    {
                        string line = "" + x.Name + ":" +
                                      x.ObjectInstantiate.transform.position.x.ToString() + "," +
                                      x.ObjectInstantiate.transform.position.y.ToString() + "," +
                                      x.ObjectInstantiate.transform.position.z.ToString() + ":" +
                                      x.ObjectInstantiate.transform.rotation.x.ToString() + "," +
                                      x.ObjectInstantiate.transform.rotation.y.ToString() + "," +
                                      x.ObjectInstantiate.transform.rotation.z.ToString() + "," +
                                      x.ObjectInstantiate.transform.rotation.w.ToString() + ":" +
                                      x.ObjectInstantiate.transform.localScale.x.ToString() + "," +
                                      x.ObjectInstantiate.transform.localScale.y.ToString() + "," +
                                      x.ObjectInstantiate.transform.localScale.z.ToString();

                        var file = new System.IO.StreamWriter(WorldEditor.Instance.SavedObjectsPath, true);
                        file.WriteLine(line);
                        file.Close();
                    }
                }

                if (GUI.Button(new Rect(10, 230, 120, 20), "CLEAR & SAVEALL"))
                {
                    File.WriteAllText(WorldEditor.Instance.SavedObjectsPath, string.Empty);
                    foreach (var x in WorldEditor.Instance.AllSpawnedObjects)
                    {
                        string line = "" + WorldEditor.Instance.PrefabBundleDictionary[x.Name] + ":" + x.Name + ":" +
                                      x.ObjectInstantiate.transform.position.x.ToString() + "," +
                                      x.ObjectInstantiate.transform.position.y.ToString() + "," +
                                      x.ObjectInstantiate.transform.position.z.ToString() + ":" +
                                      x.ObjectInstantiate.transform.rotation.x.ToString() + "," +
                                      x.ObjectInstantiate.transform.rotation.y.ToString() + "," +
                                      x.ObjectInstantiate.transform.rotation.z.ToString() + "," +
                                      x.ObjectInstantiate.transform.rotation.w.ToString() + ":" +
                                      x.ObjectInstantiate.transform.localScale.x.ToString() + "," +
                                      x.ObjectInstantiate.transform.localScale.y.ToString() + "," +
                                      x.ObjectInstantiate.transform.localScale.z.ToString();

                        var file = new System.IO.StreamWriter(WorldEditor.Instance.SavedObjectsPath, true);
                        file.WriteLine(line);
                        file.Close();
                    }
                }

                if (GUI.Button(new Rect(10, 250, 120, 20), "CLEAR & DESTROY"))
                {
                    File.WriteAllText(WorldEditor.Instance.SavedObjectsPath, string.Empty);
                    foreach (var x in WorldEditor.Instance.AllSpawnedObjects)
                    {
                        UnityEngine.Object.Destroy(x.ObjectInstantiate);
                    }

                    WorldEditor.Instance.AllSpawnedObjects.Clear();
                }

                if (GUI.Button(new Rect(10, 270, 120, 20), "LOAD FILE"))
                {
                    var last = SpawnedObject;
                    foreach (string line in File.ReadAllLines(WorldEditor.Instance.SavedObjectsPath))
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        string[] pares = line.Split(':');

                        var nombre = pares[0];
                        var loc = pares[1];
                        var qua = pares[2];
                        var siz = pares[3];

                        // Position
                        string[] locsplit = loc.ToString().Split(',');
                        float posx = float.Parse(locsplit[0]);
                        float posy = float.Parse(locsplit[1]);
                        float posz = float.Parse(locsplit[2]);

                        // Quaternion
                        string[] quasplit = qua.ToString().Split(',');
                        float quax = float.Parse(quasplit[0]);
                        float quay = float.Parse(quasplit[1]);
                        float quaz = float.Parse(quasplit[2]);
                        float quaw = float.Parse(quasplit[3]);

                        // Size
                        string[] sizsplit = siz.ToString().Split(',');
                        float sizx = float.Parse(sizsplit[0]);
                        float sizy = float.Parse(sizsplit[1]);
                        float sizz = float.Parse(sizsplit[2]);


                        TempGameObject = new GameObject();
                        SpawnedObject = TempGameObject.AddComponent<LoadingHandler.LoadObjectFromBundle>();
                        SpawnedObject.Create(nombre, WorldEditor.Instance.PrefabBundleDictionary[nombre],
                            new Vector3(posx, posy, posz), new Quaternion(quax, quay, quaz, quaw),
                            new Vector3(sizx, sizy, sizz));
                        UnityEngine.Object.DontDestroyOnLoad(TempGameObject);
                    }

                    SpawnedObject = last;
                }

                GUI.Box(new Rect(0, 310, 140, 120), "Object Control");

                if (GUI.Button(new Rect(10, 330, 120, 20), "To Me"))
                {
                    if (SpawnedObject != null && SpawnedObject.ObjectInstantiate != null)
                    {
                        SpawnedObject.ObjectInstantiate.transform.position = playerloc;
                    }
                }

                if (GUI.Button(new Rect(10, 350, 120, 20), "Reset Rot"))
                {
                    if (SpawnedObject != null && SpawnedObject.ObjectInstantiate != null)
                    {
                        SpawnedObject.ObjectInstantiate.transform.rotation = new Quaternion(0, 0, 0, 1);
                    }
                }

                if (GUI.Button(new Rect(10, 370, 120, 20), "Collider"))
                {
                    if (SpawnedObject != null && SpawnedObject.ObjectInstantiate != null)
                    {
                        Collider collider = SpawnedObject.ObjectInstantiate.collider;
                        if (collider != null)
                        {
                            SpawnedObject.ObjectInstantiate.collider.enabled =
                                !SpawnedObject.ObjectInstantiate.collider.enabled;
                        }
                    }
                }

                if (GUI.Button(new Rect(10, 390, 120, 20), "Select Object"))
                {
                    if (RustBuster2016.API.Hooks.LocalPlayer != null)
                    {
                        if (WorldEditor.Instance.AllSpawnedObjects.Count == 0)
                        {
                            Rust.Notice.Inventory("", "We got no spawned objects buddy!");
                            return;
                        }

                        //RustBuster2016.API.Hooks.LogData("test", "test12");
                        var player = RustBuster2016.API.Hooks.LocalPlayer;
                        Vector3 pos = player.controllable.character.transform.position;
                        Vector3 eyesOrigin = player.controllable.character.eyesOrigin;
                        Vector3 direction = player.controllable.character.eyesRay.direction;

                        RaycastHit[] hitArray = Physics.RaycastAll(eyesOrigin, direction, 60f);

                        LoadingHandler.LoadObjectFromBundle obj = null;
                        float dist = float.MaxValue;


                        RaycastHit closesthit = new RaycastHit();
                        closesthit.distance = -1;
                        float hitdist = float.MaxValue;
                        //RustBuster2016.API.Hooks.LogData("test", "test1233333");
                        foreach (var hit in hitArray)
                        {
                            if (hit.distance < hitdist)
                            {
                                closesthit = hit;
                                hitdist = hit.distance;
                            }
                        }

                        if (closesthit.distance >= 0f)
                        {
                            foreach (var x in WorldEditor.Instance.AllSpawnedObjects)
                            {
                                try
                                {
                                    //RustBuster2016.API.Hooks.LogData("test", "dist2");
                                    float dist2 = Vector3.Distance(closesthit.transform.position,
                                        x.ObjectInstantiate.transform.position);

                                    //RustBuster2016.API.Hooks.LogData("test", "playerdist");
                                    float playerdist = Vector3.Distance(pos, x.ObjectInstantiate.transform.position);

                                    //RustBuster2016.API.Hooks.LogData("test", "boom");
                                    if (dist2 < dist && playerdist <= 60f)
                                    {
                                        //RustBuster2016.API.Hooks.LogData("test", "boom2");
                                        dist = dist2;
                                        obj = x;
                                    }
                                }
                                catch
                                {
                                    // probably hit a rust object, avoid it.
                                }
                            }
                        }

                        if (obj != null)
                        {
                            SpawnedObject = obj;
                            Rust.Notice.Inventory("", "Found the closest object to you hopefully.");
                        }
                        else
                        {
                            Rust.Notice.Inventory("", "Couldn't find anything.");
                        }
                    }

                    /*RustBuster2016.API.Hooks.LogData("test", "test1");
                    if (RustBuster2016.API.Hooks.LocalPlayer != null)
                    {
                        RustBuster2016.API.Hooks.LogData("test", "test12");
                        var player = RustBuster2016.API.Hooks.LocalPlayer;
                        Vector3 eyesOrigin = player.controllable.character.eyesOrigin;
                        Vector3 direction = player.controllable.character.eyesRay.direction;

                        RaycastHit[] hitArray = Physics.RaycastAll(eyesOrigin, direction, 60f);

                        foreach (RaycastHit hit in hitArray)
                        {
                            RustBuster2016.API.Hooks.LogData("test", "test32 " + hit.collider.GetComponent<LoadingHandler.CustomObjectIdentifier>());
                            RustBuster2016.API.Hooks.LogData("test", "test32 " + hit.collider.gameObject);
                            RustBuster2016.API.Hooks.LogData("test", "test3 " + hit.collider.gameObject.GetComponent<LoadingHandler.CustomObjectIdentifier>());
                            if (hit.collider != null && hit.collider.gameObject != null && hit.collider.gameObject.GetComponent<LoadingHandler.CustomObjectIdentifier>() != null)
                            {
                                RustBuster2016.API.Hooks.LogData("test", "test4 " + hit.collider.gameObject.name);
                                var data = hit.collider.gameObject
                                    .GetComponent<LoadingHandler.CustomObjectIdentifier>();
                                SpawnedObject = data.BundleClass;
                            }
                        }
                    }*/
                }

                if (GUI.Button(new Rect(10, 410, 120, 20), "Toggle Animation"))
                {
                    if (SpawnedObject.ObjectInstantiate.animation.isPlaying)
                        SpawnedObject.ObjectInstantiate.animation.Stop();
                    else
                        SpawnedObject.ObjectInstantiate.animation.Play();
                }

                const string a = "LIST (LControl + LAlt)" + "\n \n" +
                                 "POSx + (NUMPAD 1)" + "\n" +
                                 "POSx - (NUMPAD 2)" + "\n" +
                                 "POSz + (NUMPAD 4)" + "\n" +
                                 "POSz - (NUMPAD 5)" + "\n" +
                                 "POSy + (NUMPAD 7)" + "\n" +
                                 "POSy - (NUMPAD 8)" + "\n \n" +
                                 "ROTx + (UP)" + "\n" +
                                 "ROTx - (DOWN)" + "\n" +
                                 "ROTy + (LEFT)" + "\n" +
                                 "ROTy - (RIGTH)" + "\n \n" +
                                 "ROTz + (NUMPAD 3)" + "\n" +
                                 "ROTz - (NUMPAD Intro)" + "\n \n" +
                                 "SIZE + (NUMPAD *)" + "\n" +
                                 "SIZE - (NUMPAD -)";
                GUI.Label(new Rect(10, 430, 600, 600), a);
            }
            catch (Exception ex)
            {
                RustBuster2016.API.Hooks.LogData("Error", "Something is wrong with the gui: " + ex);
            }
        }
    }
}