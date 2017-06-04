using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Security;

namespace Air2000
{
    public enum Profession
    {
        Warrior = 0,
        Archer = 1,
    }

    public class Character : MonoBehaviour
    {
        #region [Fields]

        #region Character Components

        public MotionMachine MotionMachine;
        public Commander Commander;
        public AnimationCrossfader AnimationCrossfader;
        public AIMachine AIMachine;
        public SkillController SkillController;
        public FlyerController FlyerController;
        public PathFinding PathFinding;

        private UnityEngine.AI.NavMeshAgent m_NavAgent;

        public UnityEngine.AI.NavMeshAgent NavAgent
        {
            get
            {
                if (m_NavAgent == null)
                {
                    m_NavAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
                }
                if (m_NavAgent == null)
                {
                    m_NavAgent = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
                }
                return m_NavAgent;
            }
        }

        #endregion Character Components

        public Profession Profession;

        public bool Moveable = true;

        [HideInInspector]
        public Quaternion pTargetRotation;

        [HideInInspector]
        public float Radius
        {
            get
            {
                if (NavAgent)
                {
                    return NavAgent.radius;
                }
                return 0.5f;
            }
        }

        public float Height
        {
            get
            {
                if (NavAgent)
                {
                    return NavAgent.height;
                }
                return 2.0f;
            }
        }

        [HideInInspector]
        public Character Enemy;

        [NonSerialized]
        public Animation Animation;

        [NonSerialized]
        public Renderer[] Renderers;

        [HideInInspector]
        public TextAsset Config;

        private EventManager m_EventProcessor;

        public EventManager EventProcessor
        {
            get
            {
                if (m_EventProcessor == null)
                {
                    m_EventProcessor = new EventManager();
                }
                return m_EventProcessor;
            }
        }

        public bool IsDie = false;
        public Player Player;

        private Transform m_ComponentsNode;

        public Transform ComponentsNode
        {
            get
            {
                if (m_ComponentsNode == null)
                {
                    m_ComponentsNode = transform.Find("Components");
                }
                if (m_ComponentsNode == null)
                {
                    m_ComponentsNode = new GameObject("Components").transform;
                    m_ComponentsNode.SetParent(transform);
                }
                return m_ComponentsNode;
            }
        }

        #endregion [Fields]

        #region [Functions]

        #region monobehaviour functions

        protected virtual void Awake()
        {
            GetAnimation();
            GetMotionMachine();
            GetCommander();
            GetAnimationCrossfader();
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        protected virtual void LateUpdate()
        {
        }

        protected virtual void OnDestroy()
        {
        }

        #endregion monobehaviour functions

        #region set & get

        public Vector3 WorldPosition
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public Vector3 WorldRotation
        {
            get { return transform.eulerAngles; }
            set
            {
                Quaternion rotation = Quaternion.Euler(value);
                transform.rotation = rotation;
            }
        }

        public Vector3 DestinationPosition
        {
            get; set;
        }

        public float DistanceToDestination(bool minusRadius = true)
        {
            return DistanceToPoint(DestinationPosition, minusRadius);
        }

        public float DistanceToPoint(Vector3 point, bool minusRadius = true)
        {
            if (minusRadius)
            {
                float distance = Vector3.Distance(WorldPosition, point) - Radius;
                return distance < 0 ? 0 : distance;
            }
            else
            {
                return Vector3.Distance(WorldPosition, point);
            }
        }

        public float DistanceToCharacter(Character character, bool minusRadius = true)
        {
            if (character == null)
            {
                return 0;
            }
            if (minusRadius)
            {
                float distance = Vector3.Distance(WorldPosition, character.WorldPosition) - Radius - character.Radius;
                return distance < 0 ? 0 : distance;
            }
            else
            {
                return Vector3.Distance(WorldPosition, character.WorldPosition);
            }
        }

        public Animation GetAnimation()
        {
            if (Animation == null)
            {
                Animation = GetComponent<Animation>();
            }
            if (Animation == null)
            {
                Animation[] animations = GetComponentsInChildren<Animation>(true);
                if (animations != null && animations.Length > 0)
                {
                    Animation = animations[0];
                }
                if (Animation != null)
                {
                    Animation.playAutomatically = false;
                    Animation.cullingType = AnimationCullingType.AlwaysAnimate;
                }
            }
            return Animation;
        }

        public MotionMachine GetMotionMachine()
        {
            if (MotionMachine == null)
            {
                MotionMachine[] machines = GetComponentsInChildren<MotionMachine>(true);
                if (machines != null && machines.Length > 0)
                {
                    MotionMachine = machines[0];
                }
            }
            return MotionMachine;
        }

        public Commander GetCommander()
        {
            if (Commander == null)
            {
                Commander[] commanders = GetComponentsInChildren<Commander>(true);
                if (commanders != null && commanders.Length > 0)
                {
                    Commander = commanders[0];
                }
            }
            return Commander;
        }

        public AnimationCrossfader GetAnimationCrossfader()
        {
            if (AnimationCrossfader == null)
            {
                AnimationCrossfader[] crossfaders = GetComponentsInChildren<AnimationCrossfader>(true);
                if (crossfaders != null && crossfaders.Length > 0)
                {
                    AnimationCrossfader = crossfaders[0];
                }
            }
            return AnimationCrossfader;
        }

        public Transform GetBodyTransform()
        {
            if (Animation == null)
            {
                Animation = GetAnimation();
            }
            if (Animation == null) return null;
            return Animation.transform;
        }

        public virtual void SetToonColor(Color varColor)
        {
            Renderers = GetRenderers();
            if (Renderers == null || Renderers.Length == 0)
            {
                return;
            }
            for (int i = 0; i < Renderers.Length; i++)
            {
                Renderer render = Renderers[i];
                if (render == null)
                {
                    continue;
                }
                Material mat = render.sharedMaterial;
                if (mat == null)
                {
                    return;
                }
                if (mat.HasProperty("_ToonColor"))
                {
                    mat.SetColor("_ToonColor", varColor);
                }
            }
        }

        public virtual void SetToonWidth(float width)
        {
            Renderer[] renderers = GetRenderers();
            if (renderers != null && renderers.Length > 0)
            {
                for (int i = 0; i < renderers.Length; i++)
                {
                    Renderer renderer = renderers[i];
                    if (renderer == null)
                    {
                        continue;
                    }
                    Material[] mats = renderer.sharedMaterials;
                    if (mats != null && mats.Length > 0)
                    {
                        for (int j = 0; j < mats.Length; j++)
                        {
                            Material mat = mats[j];
                            if (mat == null)
                            {
                                continue;
                            }
                            Shader shader = mat.shader;
                            if (shader == null)
                            {
                                continue;
                            }
                            if (IsShaderIllegal(shader))
                            {
                                continue;
                            }
                            mat.SetFloat("_Outline", width);
                        }
                    }
                }
            }
        }

        private bool IsShaderIllegal(Shader shader)
        {
            //if (shader == null)
            //{
            //    return true;
            //}
            //if (shader.name.StartsWith("Air2000/ToonShading/NormalOutline"))
            //{
            //    return false;
            //}
            return true;
        }

        public virtual void SetMaterial(string varRenderRootPath, Material varMat)
        {
            if (varMat == null)
            {
                return;
            }
            Transform body = GetBodyTransform();
            if (body == null)
            {
                return;
            }
            Transform renderTran = body.Find(varRenderRootPath);
            if (renderTran == null)
            {
                return;
            }
            SetMaterial(renderTran, varMat);
        }

        public virtual void SetMaterial(Transform varRenderTran, Material varMat)
        {
            if (varRenderTran == null)
            {
                return;
            }
            if (varMat == null)
            {
                return;
            }
            Renderer render = varRenderTran.GetComponent<Renderer>();
            if (render == null)
            {
                return;
            }
            render.sharedMaterial = varMat;
        }

        public virtual void SetTexture(Material varMat, string varItemName, Texture varTexture)
        {
            if (varMat == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(varItemName))
            {
                return;
            }
            if (varTexture == null)
            {
                return;
            }
            varMat.SetTexture(varItemName, varTexture);
        }

        public virtual void SetChActive(bool state)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(state);
            }
        }

        public virtual void SetBodyActive(bool state)
        {
            Transform bodyTran = GetBodyTransform();
            if (bodyTran != null && bodyTran.gameObject != null)
            {
                bodyTran.gameObject.SetActive(state);
            }
        }

        public virtual Renderer[] GetRenderers()
        {
            if (Renderers == null || Renderers.Length == 0)
            {
                Renderers = transform.GetComponentsInChildren<Renderer>(true);
            }
            return Renderers;
        }

        public virtual Transform GetRendererTransform(string rootpath)
        {
            if (string.IsNullOrEmpty(rootpath))
            {
                return null;
            }
            Transform bodyTran = GetBodyTransform();
            if (bodyTran == null)
            {
                return null;
            }
            return bodyTran.Find(rootpath);
        }

        public virtual Renderer GetRendererByRootpath(string rootpath)
        {
            Transform rendererTrans = GetRendererTransform(rootpath);
            if (rendererTrans == null) return null;
            return rendererTrans.GetComponent<Renderer>();
        }

        public virtual void SetRenderPower(float value)
        {
            Renderers = GetRenderers();
            if (Renderers != null && Renderers.Length > 0)
            {
                for (int i = 0; i < Renderers.Length; i++)
                {
                    Renderer renderer = Renderers[i];
                    if (renderer == null)
                    {
                        continue;
                    }
                    SetRenderPower(renderer, value);
                }
            }
        }

        public virtual void SetRenderPower(Renderer varRender, float value)
        {
            if (varRender == null)
            {
                return;
            }
            Material mat = varRender.sharedMaterial;
            if (mat == null)
            {
                return;
            }
            if (mat.HasProperty("_Strength"))
            {
                mat.SetFloat("_Strength", value);
            }
        }

        public virtual Transform GetTransformByRootPath(string rootpath)
        {
            if (string.IsNullOrEmpty(rootpath))
            {
                return GetBodyTransform();
            }
            Transform bodyTran = GetBodyTransform();
            if (bodyTran == null) return null;
            return bodyTran.Find(rootpath);
        }

        public virtual void SetPosition(Vector3 position)
        {
            if (transform != null)
            {
                transform.position = position;
            }
        }

        public virtual void SetLocalPosition(Vector3 position)
        {
            if (transform != null)
            {
                transform.localPosition = position;
            }
        }

        public virtual void SetBodyPosition(Vector3 position)
        {
            Transform bodyTran = GetBodyTransform();
            if (bodyTran != null && bodyTran.gameObject != null)
            {
                bodyTran.position = position;
            }
        }

        public virtual void SetBodyLocalPosition(Vector3 position)
        {
            Transform bodyTran = GetBodyTransform();
            if (bodyTran != null && bodyTran.gameObject != null)
            {
                bodyTran.localPosition = position;
            }
        }

        public virtual void SetRotation(Quaternion rotation)
        {
            if (transform != null)
            {
                transform.rotation = rotation;
            }
        }

        public virtual void SetLocalRotation(Quaternion rotation)
        {
            if (transform != null)
            {
                transform.localRotation = rotation;
            }
        }

        public virtual void SetBodyTilt(Quaternion rotation)
        {
            SetBodyRotation(rotation);
        }

        public virtual void SetBodyRotation(Quaternion rotation)
        {
            Transform bodyTran = GetBodyTransform();
            if (bodyTran != null && bodyTran.gameObject != null)
            {
                bodyTran.localRotation = rotation;
            }
        }

        public virtual void SetLocalScale(Vector3 scale)
        {
            if (transform != null)
            {
                transform.localScale = scale;
            }
        }

        public virtual void SetBodyScale(Vector3 scale)
        {
            Transform bodyTran = GetBodyTransform();
            if (bodyTran != null && bodyTran.gameObject != null)
            {
                bodyTran.localScale = scale;
            }
        }

        public virtual void SetParent(Transform parent)
        {
            if (transform)
            {
                transform.SetParent(parent);
            }
        }

        public virtual void TurnToTargetDirection()
        {
            TurnToTargetDirection(DestinationPosition);
        }

        public virtual void TurnToTargetDirection(Vector3 target)
        {
            Vector3 dir = target - transform.position;
            dir.Normalize();
            Quaternion rotation = Quaternion.LookRotation(dir);
            SetRotation(Quaternion.Euler(0f, rotation.eulerAngles.y, 0f));
        }

        #endregion set & get

        #region listenr of iTween

        public void OnTweenEnd(object obj)
        {
            if (obj != null)
            {
                MethodInfo info = obj.GetType().GetMethod("OnTweenEnd");
                if (info != null)
                {
                    info.Invoke(obj, new object[] { });
                }
            }
        }

        #endregion listenr of iTween

        #region attack and beattack function

        public void TriggerAttack(Character enemy)
        {
            Enemy = enemy;
            if (Enemy == null)
            {
                return;
            }
            Vector3 dir = Enemy.transform.position - transform.position;
            dir.Normalize();
            Quaternion rotation = Quaternion.LookRotation(dir);
            SetRotation(rotation);
            SetBodyRotation(Quaternion.identity);

            Enemy.transform.forward = -transform.forward;
            Enemy.SetBodyTilt(Quaternion.identity);

            ExecuteCommand(CharacterCommand.CC_Attack_1);
        }

        public void Attack(Character beAttacker = null)
        {
            if (beAttacker == null)
            {
                beAttacker = Enemy;
            }
            if (beAttacker != null)
            {
                beAttacker.ExecuteCommand(CharacterCommand.CC_BeAttack_1);
            }
        }

        public void BeAttack(Character attacker)
        {
        }

        #endregion attack and beattack function

        public void Initialize(Player player, TextAsset config)
        {
            Player = player;
            name = "Player: " + player.Data.name + "(ID:" + player.Data.roleid + ")";
            Helper.SetLayer(gameObject, LAYER.Player.ToString());
            Config = config;
            ParseXML();
            OnCharacterInitialized();
        }

        private void OnCharacterInitialized()
        {
            PathFinding.OnCharacterInitialized(this);
            AnimationCrossfader.OnCharacterInitialized(this);
            MotionMachine.OnCharacterInitialized(this);
            Commander.OnCharacterInitialized(this);
            AIMachine.OnCharacterInitialized(this);
            SkillController.OnCharacterInitialized(this);
        }

        public virtual void ParseXML()
        {
            if (Config == null || Config.text == null)
            {
                CharacterSystemUtils.LogError("Character.cs:ParseBaseConfig fail caused by null TextAsset instace");
                return;
            }
            SecurityElement element = null;
            try
            {
                element = SecurityElement.FromString(Config.text);
            }
            catch (Exception e)
            {
                CharacterSystemUtils.LogError("Character.cs:ParseBaseConfig fail caused by convert text to SecurityElement exception: " + e.Message);
            }
            if (element == null)
            {
                CharacterSystemUtils.LogError("Character.cs:ParseBaseConfig fail caused by null SecurityElement instace");
                return;
            }

            SecurityElement navElement = element.SearchForChildByTag("NavAgent");
            if (navElement != null)
            {
                if (PathFinding != null)
                {
                    GameObject.DestroyImmediate(PathFinding);
                }
                PathFinding = gameObject.AddComponent<PathFinding>();
                PathFinding.ParseXML(navElement, this);
            }

            SecurityElement motionsElement = element.SearchForChildByTag("Motions");
            if (motionsElement != null)
            {
                if (MotionMachine == null)
                {
                    Transform motionConfigNode = ComponentsNode.Find("MotionMachine");
                    if (motionConfigNode == null)
                    {
                        motionConfigNode = new GameObject("MotionMachine").transform;
                        motionConfigNode.transform.SetParent(ComponentsNode);
                    }
                    MotionMachine = motionConfigNode.gameObject.AddComponent<MotionMachine>();
                }
                MotionMachine.ParseXML(motionsElement, this);
            }
            SecurityElement animationCrossfaderElement = element.SearchForChildByTag("AnimationCrossfader");
            if (animationCrossfaderElement != null)
            {
                if (AnimationCrossfader == null)
                {
                    Transform animationCrossfaderNode = ComponentsNode.Find("AnimationCrossfader");
                    if (animationCrossfaderNode == null)
                    {
                        animationCrossfaderNode = new GameObject("AnimationCrossfader").transform;
                        animationCrossfaderNode.transform.SetParent(ComponentsNode);
                    }
                    AnimationCrossfader = animationCrossfaderNode.gameObject.AddComponent<AnimationCrossfader>();
                }
                AnimationCrossfader.ParseXML(animationCrossfaderElement, this);
            }

            SecurityElement commanderElement = element.SearchForChildByTag("Commander");
            if (commanderElement != null)
            {
                if (Commander == null)
                {
                    Transform commanderNode = ComponentsNode.Find("Commander");
                    if (commanderNode == null)
                    {
                        commanderNode = new GameObject("Commander").transform;
                        commanderNode.transform.SetParent(ComponentsNode);
                    }
                    Commander = commanderNode.gameObject.AddComponent<Commander>();
                }
                Commander.ParseXML(commanderElement, this);
            }

            SecurityElement aiElement = element.SearchForChildByTag("AIMachine");
            if (aiElement != null)
            {
                if (AIMachine == null)
                {
                    Transform aimachineNode = ComponentsNode.Find("AIMachine");
                    if (aimachineNode == null)
                    {
                        aimachineNode = new GameObject("AIMachine").transform;
                        aimachineNode.transform.SetParent(ComponentsNode);
                    }
                    AIMachine = aimachineNode.gameObject.AddComponent<AIMachine>();
                }
                AIMachine.ParseXML(aiElement, this);
            }

            SecurityElement skillControllerElement = element.SearchForChildByTag("SkillController");
            if (skillControllerElement != null)
            {
                if (SkillController == null)
                {
                    Transform skillControllerNode = ComponentsNode.Find("SkillController");
                    if (skillControllerNode == null)
                    {
                        skillControllerNode = new GameObject("SkillController").transform;
                        skillControllerNode.transform.SetParent(ComponentsNode);
                    }
                    SkillController = skillControllerNode.gameObject.AddComponent<SkillController>();
                }
                SkillController.ParseXML(skillControllerElement, this);
            }

            ExecuteCommand(CharacterCommand.CC_Idle);
        }

        public virtual void PoatCharacterCommand(CharacterCommand command)
        {
        }

        /// <summary>
        /// 同步角色的位置，朝向等信息.
        /// Sync character's position,rotation and etc..
        /// </summary>
        /// <param name="position"></param>
        /// <param name="eulerAngles"></param>
        public virtual void PostCharacterTransform(Vector3 position, Vector3 eulerAngles, CharacterCommand command = CharacterCommand.None)
        {
            if (Constants.IsSinglePlayer)
            {
                //PathFinding.SetDestination(position);
                //NavAgent.Move(position);
                WorldRotation = eulerAngles;
            }
            else if (Constants.IsSinglePlayer == false && Player.PlayerType == PlayerType.PT_Hero)
            {
                if (Constants.CURRENT_ROOM_INFO != null)
                {
                    NetManager.SendNetPacket<PBMessage.go_charactercommand_return>((int)AccountMessage.GO_POSITION_COMMANF_REQUEST, new PBMessage.go_charactercommand_return()
                    {
                        fbid = Constants.CURRENT_ROOM_INFO.fbid,
                        copyid = Constants.COPY_ID,
                        roleid = Player.Data.roleid,
                        x = (int)(position.x * 100000f),
                        y = (int)(position.y * 100000f),
                        z = (int)(position.z * 100000f),
                        face_x = (int)(eulerAngles.x * 10000),
                        face_y = (int)(eulerAngles.y * 10000),
                        face_z = (int)(eulerAngles.z * 10000),
                        action = (int)command,
                        latency = NetManager.CurrentLatency,
                    });
                }
            }
        }

        private float m_LastSendTime = -1f;
        private float m_SyncInterval = 0.5f;

        protected virtual void Update()
        {
            if (NeedSync)
            {
                iTween.MoveTo(gameObject, m_TargetPosition, 0.2f);
                //WorldPosition = Vector3.Lerp(WorldPosition, m_TargetPosition, Time.deltaTime * 14.0f);
                //Quaternion tempRotation = Quaternion.Euler(m_TargetEulerAngles);
                //pTargetRotation = Quaternion.Lerp(transform.rotation, tempRotation, 1.0f * Time.deltaTime  );
                NeedSync = false;
            }
        }

        public bool ExecuteCommand(CharacterCommand command, bool dontSync = false)
        {
            return Commander.ExecuteCommand(command);
            if (command == CharacterCommand.CC_Skill_2)
            {
                int a = 1;
            }
            if (dontSync)
            {
                return Commander.ExecuteCommand(command);
            }
            if (Constants.IsSinglePlayer)
            {
                return Commander.ExecuteCommand(command);
            }
            else if (Constants.IsSinglePlayer == false && Player.PlayerType == PlayerType.PT_Hero)
            {
                PostCharacterTransform(WorldPosition, WorldRotation, command);
            }
            return true;
        }

        public virtual void SyncTransform(PBMessage.go_charactercommand_return transformInfo)
        {
            if (transformInfo == null)
            {
                return;
            }
            Vector3 position = new Vector3(transformInfo.x / 100000f, transformInfo.y / 100000f, transformInfo.z / 100000f);

            //if(Player.PlayerType == PlayerType.PT_Hero)
            {
                //1
                // WorldPosition = position;
            }

            //2
            //m_TargetPosition = position;
            //NeedSync = true;

            //3
            //NavAgent.Move(position);

            //4
            //PathFinding.SetDestination(position);
            Vector3 eulerAngles = new Vector3(transformInfo.face_x / 10000f, transformInfo.face_y / 10000f, transformInfo.face_z / 10000f);
            WorldRotation = eulerAngles;
            CharacterCommand command = (CharacterCommand)transformInfo.action;
            if (command != CharacterCommand.None)
            {
                WorldPosition = position;
            }
            ExecuteCommand(command, true);
        }

        public bool NeedSync = false;
        public Vector3 m_TargetPosition;
        public Vector3 m_TargetEulerAngles;

        #endregion [Functions]
    }
}