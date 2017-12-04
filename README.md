# UnityProjectWithUGF
A common Unity template Project base on UnityGameFramework.

---

>If you have any question about this **Template**, go see [UnityGameFramework](https://github.com/EllanJiang/UnityGameFramework)'s demo: [StarForce](https://github.com/EllanJiang/StarForce).

### Some Tips for Users

> * 插件类的 **`.dll`** 文件放在 **`Assets/GameMain/Libraries/`** 下。
> * 配置文件放在 **`Assets/GameMain/Configs/`** ，用于构建 **AssetBundle** 或者用语游戏加载基本配置以及版本信息。
> * 版本信息文件 **`Assets/GameMain/Configs/BuildInfo.txt`** ，需要设置检查版本信息的Url和各个平台下载App的Url，服务端存储的版本信息的类型 **`VersionInfo`** 。在 **`VersionInfo.GameUpdateUrl`** 地址下存储资源更新的目录，目录结构基本为版本信息/平台信息，详情见代码。
> * 游戏的配置表可以放在 **`Assets/GameMain/DataTables/`** ，目前为由Excel导出的 **.txt** 配置表，不被解析的行用`#`标记行首。以后版本可能会支持加载 **.json** 配置表。配置表对应的类型文件放在 **`Assets/GameMain/Scripts/DataTable/`** 下。
> *  **`Assets/GameMain/Entities/`** 文件夹下存放各种在游戏内生成和布置的各种 **.prefab** 文件。
> *  **`Assets/GameMain/Scripts/Game/`** 文件夹下存放游戏模式或者游戏玩法的脚本文件。
> * 前后端传输协议类型声明文件(即包类型)放在 **`Assets/GameMain/Scripts/Network/Packet/`** 下(限Socket)。
> * 如果使用的是WebSocket来进行前后端交互，则将protobuf生成的.cs文件放在 **`Assets/GameMain/Scripts/NetworkCustom/Protos`** 下, **`Assets/GameMain/Scripts/NetworkCustom/ProtoHandlers`** 下放各种实现的对后端消息进行处理的Handler。虽然支持WebSocket的通信，但是并不支持WebGL平台下的通信，还是有待扩充。
> * UI逻辑脚本放在 **`Assets/GameMain/Scripts/UI/`** 内，继承 **`UGuiForm`** 类。
> * 添加了 **`Unity-UI-Extensions`** 扩展脚本集，方便UI的制作，不用再从头造轮子。源项目：**[Unity-UI-Extensions](https://bitbucket.org/UnityUIExtensions/unity-ui-extensions)**
> * 注意代码风格和格式，参考 **[UGF的Demo](https://github.com/EllanJiang/StarForce)**

## Still Updating...
> ### Some **[Plans](https://www.teambition.com/project/5a0aa7bdb76120769b0e4caf/tasks/scrum/5a0aa7bdb76120769b0e4cb3)**

> ***Email: <allendk@foxmail.com>***