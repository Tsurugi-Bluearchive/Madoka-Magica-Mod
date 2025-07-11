﻿using MadokaMagica.Modules.Characters;
using MadokaMagica.Modules;
using MadokaMagica.Zilu.SkillStates;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using MadokaMagica.Zilu.Content;
using MadokaMagica.Zilu.SkillStates.BaseStates;
namespace MadokaMagica.Zilu
{
    public class ZiluSurvivor : SurvivorBase<ZiluSurvivor>
    {
        //used to load the assetbundle for this character. must be unique
        public override string assetBundleName => "mamiassetbundle"; //if you do not change this, you are giving permission to deprecate the mod

        //the name of the prefab we will create. conventionally ending in "Body". must be unique
        public override string bodyName => "MamiBody"; //if you do not change this, you get the point by now

        //name of the ai master for vengeance and goobo. must be unique
        public override string masterName => "MamiMonsterMaster"; //if you do not

        //the names of the prefabs you set up in unity that we will use to build your character
        public override string modelPrefabName => "mdlMami";
        public override string displayPrefabName => "MamiDisplay";

        public const string MAMI_PREFIX = MagicaPlugin.DEVELOPER_PREFIX + "_MAMI_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => MAMI_PREFIX;
        
        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = MAMI_PREFIX + "NAME",
            subtitleNameToken = MAMI_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("texMAMIIcon"),
            bodyColor = Color.white,
            sortPosition = 100,

            crosshair = Asset.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 120f,
            healthRegen = 1.5f,
            armor = 0f,

            jumpCount = 5,
        };

        public override CustomRendererInfo[] customRendererInfos => [];

        public override UnlockableDef characterUnlockableDef => ZiluUnlockables.characterUnlockableDef;
        
        public override ItemDisplaysBase itemDisplays => new ZiluItemDisplays();

        //set in base classes
        public override AssetBundle assetBundle { get; protected set; }

        public override GameObject bodyPrefab { get; protected set; }
        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }
        public override GameObject displayPrefab { get; protected set; }

        public Component MamiGun;

        public static SkillDef reload;

        public static SkillDef precisionStrike;
        public static SkillDef ceaselessBarage;

        public override void Initialize()
        {
            //uncomment if you have multiple characters
            //ConfigEntry<bool> characterEnabled = Config.CharacterEnableConfig("Survivors", "Henry");

            //if (!characterEnabled.Value)
            //    return;

            base.Initialize();
        }

        public override void InitializeCharacter()
        {
            //need the character unlockable before you initialize the survivordef
            ZiluUnlockables.Init();

            base.InitializeCharacter();

            ZiluConfig.Init();
            ZiluStates.Init();
            ZiluTokens.Init();

            ZiluAssets.Init(assetBundle);
            ZiluBuffs.Init(assetBundle);

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            AddHooks();
        }

        private void AdditionalBodySetup()
        {
            AddHitboxes();
            //bodyPrefab.AddComponent<HuntressTracerComopnent>();
            //anything else here
        }

        public void AddHitboxes()
        {
        }

        public override void InitializeEntityStateMachines() 
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //the main "Body" state machine has some special properties
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(ZiluCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            //if you set up a custom main characterstate, set it up here
            //don't forget to register custom entitystates in your HenryStates.cs

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }

        #region skills
        public override void InitializeSkills()
        {
            //remove the genericskills from the commando body we cloned
            Skills.ClearGenericSkills(bodyPrefab);
            //add our own
            AddPassiveSkill();
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtiitySkills();
            AddSpecialSkills();
        }

        //skip if you don't have a passive
        //also skip if this is your first look at skills
        private void AddPassiveSkill()
        {
            //option 1. fake passive icon just to describe functionality we will implement elsewhere
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill
            {
                enabled = true,
                skillNameToken = MAMI_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = MAMI_PREFIX + "PASSIVE_DESCRIPTION",
                keywordToken = "KEYWORD_STUNNING",
                icon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
            };
        }

        //if this is your first look at skilldef creation, take a look at Secondary first
        private void AddPrimarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Primary);

            //the primary skill is created using a constructor for a typical primary
            //it is also a SteppedSkillDef. Custom Skilldefs are very useful for custom behaviors related to casting a skill. see ror2's different skilldefs for reference
            precisionStrike = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "PrecisionStrike",
                skillNameToken = MAMI_PREFIX + "PRIMARY_GUN_NAME",
                skillDescriptionToken = MAMI_PREFIX + "PRIMARY_GUN_DESCRIPTION",
                keywordTokens = ["KEWORD_IMPLANT"],
                skillIcon = assetBundle.LoadAsset<Sprite>("fir"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.PrecisionStrkie)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = float.MaxValue,

                rechargeStock = 0,
                requiredStock = 1,
                stockToConsume = 1,
                baseMaxStock = 2,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,

            });


            Skills.AddPrimarySkills(bodyPrefab, precisionStrike);
        }

        private void AddSecondarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Secondary);

            //here is a basic skill def with all fields accounted for
            ceaselessBarage = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "CeaseleassBarage",
                skillNameToken = MAMI_PREFIX + "SECONDARY_BARAGE_NAME",
                skillDescriptionToken = MAMI_PREFIX + "SECONDARY_BARRAGE_DESCRIPTION",
                keywordTokens = ["KEWORD_IMPLANT"],
                skillIcon = assetBundle.LoadAsset<Sprite>("brrag"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.CeaselessBarrage)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = float.MaxValue,

                rechargeStock = 0,
                requiredStock = 1,
                stockToConsume = 0,
                baseMaxStock = 6,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,

            });

            reload = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "PrecisionReload",
                skillNameToken = MAMI_PREFIX + "PRIMARY_GUN_NAME",
                skillDescriptionToken = MAMI_PREFIX + "PRIMARY_GUN_DESCRIPTION",
                keywordTokens = ["KEWORD_IMPLANT"],
                skillIcon = assetBundle.LoadAsset<Sprite>("rloead"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Reload)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = float.MaxValue,
                rechargeStock = 0,
                requiredStock = 1,
                stockToConsume = 0,
                baseMaxStock = 6,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });


            Skills.AddSecondarySkills(bodyPrefab, ceaselessBarage);
        }

        private void AddUtiitySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Utility);

            //here's a skilldef of a typical movement skill.
            var utilitySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryRoll",
                skillNameToken = MAMI_PREFIX + "UTILITY_ROLL_NAME",
                skillDescriptionToken = MAMI_PREFIX + "UTILITY_ROLL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SpawnGun)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
               
                baseRechargeInterval = float.MaxValue,
                rechargeStock = 0,
                requiredStock = 1,
                stockToConsume = 0,
                baseMaxStock = 4,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });

            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef1);
        }

        private void AddSpecialSkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Special);

            //a basic skill. some fields are omitted and will just have default values
            var specialSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "MamiBlast",
                skillNameToken = MAMI_PREFIX + "SPECIAL_BLAST_NAME",
                skillDescriptionToken = MAMI_PREFIX + "SPECIAL_BLAST_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("blas"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.PrecisionBlast)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon", interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 10f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1);
        }
        #endregion skills
        
        #region skins
        public override void InitializeSkins()
        {
            var skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            var defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            var skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            var defaultSkin = Skins.CreateSkinDef("DEFAULT_SKIN",
                assetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
                //pass in meshes as they are named in your assetbundle
            //currently not needed as with only 1 skin they will simply take the default meshes
                //uncomment this when you have another skin
            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySword",
            //    "meshHenryGun",
            //    "meshHenry");

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin
            
            ////creating a new skindef as we did before
            //SkinDef masterySkin = Modules.Skins.CreateSkinDef(MAMI_PREFIX + "MASTERY_SKIN_NAME",
            //    assetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
            //    defaultRendererinfos,
            //    prefabCharacterModel.gameObject,
            //    HenryUnlockables.masterySkinUnlockableDef);

            ////adding the mesh replacements as above. 
            ////if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            //masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySwordAlt",
            //    null,//no gun mesh replacement. use same gun mesh
            //    "meshHenryAlt");

            ////masterySkin has a new set of RendererInfos (based on default rendererinfos)
            ////you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            //masterySkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");

            ////here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            //masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            //{
            //    new SkinDef.GameObjectActivation
            //    {
            //        gameObject = childLocator.FindChildGameObject("GunModel"),
            //        shouldActivate = false,
            //    }
            //};
            ////simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            //skins.Add(masterySkin);
            
            #endregion

            skinController.skins = skins.ToArray();
        }
        #endregion skins

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            ZiluAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AddHooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {

        }
    }
}