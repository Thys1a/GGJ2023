using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UFramework
{
    public class UIManager : MonoBehaviour
    {
        /* 字段 */
        private static UIManager instance = null;

        private Transform _CanvasTransfrom = null;
        private Transform _NormalTransfrom = null;
        private Transform _UIScriptsTransfrom = null;
        private Transform _UIMaskTransform = null;

        //UI窗体预设路径(参数1：窗体预设名称，2：表示窗体预设路径)
        private Dictionary<string, string> _DicFormsPaths;
        //缓存所有UI窗体
        private Dictionary<string, BaseUIForm> _DicAllUIForms;
        //当前显示的UI窗体
        private Dictionary<string, BaseUIForm> _DicCurrentUIForms;

        
        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UIManager();
                }
                return instance;
            }
        }

        private void Awake()
        {


            _DicAllUIForms = new Dictionary<string, BaseUIForm>();
            _DicCurrentUIForms = new Dictionary<string, BaseUIForm>();
            _DicFormsPaths = new Dictionary<string, string>();

            InitRootCanvas();


            if (_DicFormsPaths != null)//载入预设
            {
            
                Debug.Log( " 开始解析UI。");
                //List<System.Xml.XmlNode> nodes = XMLUtil.GetChildNodes(XMLUtil.LoadXml(ResourceManager.Instance.GetGlobalconfig.uiprefabPath));
                //if (nodes == null)
                //{
                //    Debug.Log("UI：Null NodeList.");
                //}
                //foreach (System.Xml.XmlNode node in nodes)
                //{
                //    string[] item = XMLUtil.GetNameValueOfNode(node);
                //    _DicFormsPaths.Add(item[0], item[1]);
                //}
                //_DicFormsPaths.Add("ClueSelectedPanel", @"UIPrefab\clueSelectedPanel");
                //_DicFormsPaths.Add("StaffPanel", @"UIPrefab\StaffPanel");
                //_DicFormsPaths.Add("CluePanel", @"UIPrefab\cluePanel");
                //_DicFormsPaths.Add("StartPanel", @"UIPrefab\StartPanel");
            }
        }

        /// <summary>
        /// 通过名称显示ui
        /// </summary>
        /// <param name="uiFormName"></param>
        public void ShowUIForm(string uiFormName)
        {
            BaseUIForm baseUIForm = null;//UI窗体基类

            if (string.IsNullOrEmpty(uiFormName)) return;
            //根据UI窗体的名称，加载到“所有UI窗体”缓存集合中
            baseUIForm = LoadFormsToAllUIFormsCache(uiFormName);
            if (baseUIForm == null) return;

            LoadUIToCurrentCache(uiFormName);
            Debug.Log(1);
        }

        /// <summary>
        /// 关闭当前UI窗体
        /// </summary>
        public void CloseUIForms(string strUIFormName)
        {
            BaseUIForm baseUIForm = null;                   //UI窗体基类

            /* 参数检查 */
            if (string.IsNullOrEmpty(strUIFormName)) return;
            //“所有UI窗体缓存”如果没有记录，则直接返回。
            _DicAllUIForms.TryGetValue(strUIFormName, out baseUIForm);
            if (baseUIForm == null) return;
            ExitUIFormsCache(strUIFormName); 
        
        }
        public void SetUIMask(GameObject ui)
        {
            Color newColor = _UIMaskTransform.GetComponent<Image>().color;
        
            _UIMaskTransform.GetComponent<Image>().color = new Color(255 / 255F, 255 / 255F, 255 / 255F, 0F / 255F);
            _UIMaskTransform.gameObject.SetActive(true);

            _UIMaskTransform.SetAsLastSibling();
            ui.transform.SetAsLastSibling();

        }
        public void CancelUIMask(GameObject ui)
        {
            //顶层窗体上移
            ui.transform.SetAsFirstSibling();
            //禁用遮罩窗体
            if (_UIMaskTransform.gameObject.activeInHierarchy)
            {
                //隐藏
                _UIMaskTransform.gameObject.SetActive(false);
            }
        }


        private void InitRootCanvas()
        {
            //ResourcesMgr.GetInstance().LoadAsset(SysDefine.SYS_PATH_CANVAS, false);
            //GameObject obj = Instantiate((GameObject)Resources.Load(ResourceManager.Instance.GetGlobalconfig.canvasPath));
            _CanvasTransfrom = this.transform;
            _NormalTransfrom = _CanvasTransfrom.Find("UI");
            _UIMaskTransform = _NormalTransfrom.Find("MaskPanel");
            _UIMaskTransform.gameObject.SetActive(false);

            this.gameObject.transform.SetParent(_UIScriptsTransfrom, false);

            //DontDestroyOnLoad(_CanvasTransfrom);
        }

        private BaseUIForm LoadFormsToAllUIFormsCache(string uiFormName)
        {
            /// 根据UI窗体的名称，加载到“所有UI窗体”缓存集合中
            /// 功能： 检查“所有UI窗体”集合中，是否已经加载过，否则才加载。
            BaseUIForm baseUIResult = null;                 //加载的返回UI窗体基类

            _DicAllUIForms.TryGetValue(uiFormName, out baseUIResult);
            if (baseUIResult == null)
            {
                //加载指定名称的“UI窗体”
                baseUIResult = LoadUIForm(uiFormName);
            }
            return baseUIResult;
        }
        private BaseUIForm LoadUIForm(string uiFormName)
        {
            //加载UI窗体到缓存集合中
            string UIpath = null;                   //UI窗体路径
            GameObject UIfab = null;             //创建的UI克隆体预设
            BaseUIForm baseUIForm = null;                     //窗体基类


            //根据UI窗体名称，得到对应的加载路径
            _DicFormsPaths.TryGetValue(uiFormName, out UIpath);
            //根据“UI窗体名称”，加载“预设克隆体”
            if (!string.IsNullOrEmpty(UIpath))
            {
                UIfab = Instantiate((GameObject)Resources.Load(UIpath));
            }
            //设置“UI克隆体”的父节点（根据克隆体中带的脚本中不同的“位置信息”）
            if (_CanvasTransfrom != null && UIfab != null)
            {
                baseUIForm = UIfab.GetComponent<BaseUIForm>();
                if (baseUIForm == null)
                {
                    Debug.Log("baseUiForm==null! ,请先确认窗体预设对象上是否加载了baseUIForm的子类脚本！ 参数 uiFormName=" + uiFormName);
                    return null;
                }
                UIfab.transform.SetParent(_NormalTransfrom, false); 
            

                //设置隐藏
                UIfab.SetActive(false);
                //把克隆体，加入到“所有UI窗体”（缓存）集合中。
                _DicAllUIForms.Add(uiFormName, baseUIForm);
                return baseUIForm;
            }
            else
            {
                Debug.Log("_TraCanvasTransfrom==null Or goCloneUIPrefabs==null!! ,Plese Check!, 参数uiFormName=" + uiFormName);
            }

            Debug.Log("出现不可以预估的错误，请检查，参数 uiFormName=" + uiFormName);
            return null;
        }

        private void LoadUIToCurrentCache(string uiFormName)
        {
            BaseUIForm baseUIForm;                          //UI窗体基类
            BaseUIForm baseUIFormFromAllCache;              //从“所有窗体集合”中得到的窗体

            //如果“正在显示”的集合中，存在整个UI窗体，则直接返回
            _DicCurrentUIForms.TryGetValue(uiFormName, out baseUIForm);
            if (baseUIForm != null) return;

            //把当前窗体，加载到“正在显示”集合中
            _DicAllUIForms.TryGetValue(uiFormName, out baseUIFormFromAllCache);
            if (baseUIFormFromAllCache != null)
            {
                _DicCurrentUIForms.Add(uiFormName, baseUIFormFromAllCache);
                baseUIFormFromAllCache.Display();           //显示当前窗体
            }
        }

        /// <summary>
        /// 卸载UI窗体从“当前显示窗体集合”缓存中。
        /// </summary>
        /// <param name="strUIFormName"></param>
        private void ExitUIFormsCache(string strUIFormName)
        {
            BaseUIForm baseUIForm;                        //UI窗体基类

            //“正在显示UI窗体缓存”集合没有记录，则直接返回。
            _DicCurrentUIForms.TryGetValue(strUIFormName, out baseUIForm);
            if (baseUIForm == null) return;

            //指定UI窗体，运行隐藏状态，且从“正在显示UI窗体缓存”集合中移除。
            baseUIForm.Hiding();
            _DicCurrentUIForms.Remove(strUIFormName);
        }

    }
}

