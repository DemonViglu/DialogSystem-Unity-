# DialogSystem(Unity)
 一个简小但是足够满足一定基础的对话系统。

## 初始使用介绍
### 初始设置步骤：
  ![项目总览](https://github.com/DemonViglu/SchoolWork/blob/main/Image/TempFile/1.png)<br/>
1. 找到DialogSystem\Example\DialogSystem，该场景为示例场景。也可以将DialogSystem\Example\Prefab中的 _DMM_ 拖拽到你的一个空物体上，进行调试。
2. 点击 _Add_ 按钮，即可看到一段对话播放。该对话为示例对话，包含了基础的头像及其名字显示和对话树功能实现，选项实现，和选项事件实现... ...而使用者在使用时，只需对 _UI_ 和 _SO_ 进行调整，即可放入具体的项目中进行使用。下文将会介绍如何快速上手**DemonViglu_DialogSystem**。


### 创建第一个DialogMissionSO:
- 对于一段对话（如果有选项，则选项前后各自为一段对话，下文会有解释），我们称之为一个 _DialogSystemMission_ ，该类为C语言可序列化类。而为了方便配置使用，笔者支持使用 _SriptableObject_ 进行对话的记录，称之为 _DialogSystemMissionSO_。二者近乎无差别，但当使用者近使用C类进行对话输入时，会有特殊注意点。但此处不讨论。
- _DialogSystemMissionSO_ 有一个上级管理层， _DialogSystemMissionSOManager_， 用于记录一个NPC（一个DialogSystemManager）的全部对话。<br/>

1. 创建一个文件夹，名字叫 _DialogSource_ 。右键文件夹，Create-> DialogMission-> New DialogMissionSOManager, 命名为 _第一个NPC的对话_ 。<br/>
![创建SO](https://github.com/DemonViglu/SchoolWork/blob/main/Image/TempFile/CreateSO.jpg)
2. 在该文件夹下，右键文件夹， Create->DialogMission-> New DialogMissionSO, 并命名为 _第一段对话_。 并将该 _SO_ 拖拽到刚刚的 _SOManager_ 下，并记录其在 _SOManager_ 中的列表位置。由于刚才创建的为第一个 _SO_， 则列表下标位置为0。<br/>
![放置SO](https://github.com/DemonViglu/SchoolWork/blob/main/Image/TempFile/%E6%94%BE%E7%BD%AESO.png)

3. _SO_ 如图所示，共有五个变量，分别为 _TextString_,_TextAsset_,_OptionMissionIndex_,_EventIndex_。在只需要播放一段对话事，我们只需要填充文本至两个变量中一个即可。（当 _TextString_ 不为空时，才会查找 _TextAsset_ 中的文件。默认先获取 _TextString_ 文件）。我们可以向其打入 `Hello World`。<br/>
![ShowSO_1](https://github.com/DemonViglu/SchoolWork/blob/main/Image/TempFile/Show_1.png)

4. 一段话可能有很多句话，而每一句话会分开独立显示。所以，当你希望下一句话刷新对话面板时，请在这两句话中间使用换行。比如：<br/> 
![ShowSO_2](https://github.com/DemonViglu/SchoolWork/blob/main/Image/TempFile/Show_2.png)<br/>
`Hello` <br/>
`World` <br/>
那么待会输出时，面板会先输出`Hello`，然后清空面板，再输出`World`。


### 播放刚刚创建的第一个DialogMissionSO:
1. 点击play按钮开始运行当前场景，点击预制体中的 _Add_ 按钮，会发现panel组件自动亮起，并且播放了刚刚的对话。并在结束之后自动关闭panel组件。至此，播放一句对话的基本功能已经实现完毕。<br/>
![PlayExample](https://github.com/DemonViglu/SchoolWork/blob/main/Image/TempFile/PlayerExample.jpg)<br/>
（由于该为示例场景play，所以会有头像功能出现）

## 如何实现连续的对话树功能
### DialogMissionSO 两个List变量介绍:
- Option Mission Index<br/>
   记录该对话结束之后会需要播放的`MissionSO`下标（在`DialogMissionSOManager`下的下标，如上文提到的下标0）。例如，在第一个创建的 `DialogMissionSO`的`OptionMissionIndex`列表下添加1和2，则表明该对话会有两个选项，分别每一个选项分别对应`DialogMissionSOManager`中列表下标为1的对话SO和下标2的对话SO。<BR/>**注意， 刚刚只创建了一个 _SO_ ,所以需要你自行再创建两个`SO`并添加到`DialogMissionSOManager`当中。**
- Option Description <br/>
   记录该Mission的对话选项会是什么文字内容。例如：“_选项1_”,“_选项2_”。<br/>
   **注意：两者的列表顺序以及个数应当匹配，不然会出现播放错误或异常。**
### 创建新的两个DialogMissionSO:
- 仿照上文的第一个创建方式,分别命名为“Mission_2”和“Mission_3”,并添加到**DialogMissionSOManager**下。
- 将创建的第一个DialogMissionSo的OptionMissionIndex列表下添加1和2，并且添加两个Option Description，分别为“Mission_2”和“Mission_3”。
### 播放对话树：
- 点击play按钮开始运行当前场景，点击 _AddMissionSO_ 按钮，会发现第一段对话播放结束后，场景中点亮了两个按钮，上面的文字分别为“Mission_2”和“Mission_3”。至此，我们已经尝试了作为对话树的所有基本功能。<br/>


## 如何实现在触发选项的同时触发相应的事件（方法1）：
### DialogMissionSO 的 eventIndex介绍：
1. 该默认值应为-1，表示触发该对话的时候没有任何事件发生。如果其不为-1，则表明该对话任务在**DialogMissionEventHandler**有相应的**DialogMissionEvent**需要调用，并且会调用下标为eventIndex的**DialogMissionEvent**。
### 创建DialogMissionEvent：
1. 推荐在Hierarchy窗口面板下创建一个空物体，并命名为"MissionEvent_1",并添加`DialogMissionEvent`组件。
2. 将要在该对话任务调用的函数加入到该组件下 `OptionEvent`中。<br/>
 - 创建一个空物体，并挂载代码`DemonViglu_Test_Example`脚本，并将刚刚创建 `DialogSystemManager`拖拽到脚本组件上。可以将该代码中的 `UseObjectCallBack`挂载上去。 

3. 将该物体“MissionEvent_1”拖拽至物体DialogManager的**DialogMissionEventHandler**下。由于是第一个任务，所以应当其下标为**0**。
4. 若设置在播放Mission_3的时候调用该模块事件，只需要找到相应的SO，并将其EventIndex修改为该下标0。
### 实现该模块：
- 点击play按钮开始运行当前场景，点击 _AddMissionSO_ 按钮，会发现第一段对话播放，并且场景中点亮了两个按钮，上面的文字分别为“Mission_2”和“Mission_3”,点击Mission_3，会发现即可调用刚刚所设置的函数。
## 如何实现在触发选项的同时触发相应的事件（方法2）：
### 选项广播：
 - 创建一个空物体，并挂载代码`DemonViglu_Test_Example`脚本，并将刚刚创建 `DialogSystemManager`拖拽到脚本组件上。
 - 双击该脚本，会发现在`Start`中监听了三个事件。此处我们需要监听选项，所以关注事件 `m_dialogSystemManager.missionEventHandler._OnOptionClick+=ClickOption`，该事件类型为 `event action<int,int>`，所以需要挂载符合参数类型的事件。其参数分别为 `SOIndex`,和`optionIndex`，表明是第几段对话的第几个选项被点击而触发的广播。<br/>
  **注意：第几是从0开始数起**

## 对话结束广播：
- 创建一个空物体，并挂载代码`DemonViglu_Test_Example`脚本，并将刚刚创建 `DialogSystemManager`拖拽到脚本组件上。
 - 双击该脚本，在`Start`中监听了三个事件。剩余两个时间分别为`_OnEveryMissionEnd` 和 `_OnMissionTreeEnd`。 
### OnEveryMissionEnd:
在一段对话结束时就会触发，类型为`event action<int>`，参数为当前结束的 `SO`在`SOManager`的下标index。
### OnMissionTreeEnd:
在一段对话树结束时就会触发，类型为`event action<int>`，参数为当前结束的 `SO`在`SOManager`的下标index。与上文不同。若在对话时有选项触发，则此处认为是MissionEnd,而不认为是MissionTreeEnd。所以不会触发该广播

## DialogSystem 的跳过对话功能和自动播放对话功能。
- 在示例场景中的右上角有个名为`Auto`的button。点击它，则`DialogSystem`会进入`Not_Auto`（手动播放对话状态）。
### PassToNextSentence
- 若触发该Key,且当前状态为：这句话播放完了，暂留等下一句话，则会加速跳转到下一句话(减少<a href ="#_sentenceTimeGap">_sentenceTimeGpe</a>)。且当当前状态为手动时，只有点击该Key，才会播放下一句话，否则会一直滞留。
### PassTheSentence
- 若触发该Key，且当前状态为：这句话还在播放，还有word没有打出，则会将该句子立马完整打出，并进入稍后播放下一句话的间隔（<a href="#_sentenceTimeGap">_sentenceTimeGap</a>）
## DialogSystem 的 Icon以及 Icon名字显示使用。
- 创建一个IconSOManager,并将其拖拽到你所配置的`DialogSystemManager`当中。在该`IconSOManager`中，按顺序放置名字和图片。其中名字有固定语法`XX:NUMBER`。XX表示你希望`DialogSystemManager`打印在 _UI_ 上的头像对应名字，NUMBER用于区分一个角色的不同表情头像。<br/>
  ![IconSO](https://github.com/DemonViglu/SchoolWork/blob/main/Image/TempFile/IconSO.png)
- 在文本中输入特定字符或名字来调用图片。
  在文本中的独立一行中打入`XX:NUMBER`，则`DialogSystemManager`会立即切换头像并打印名字。若后文非名字语句，则头像会有所保留至该段话结束(MissionEnd)。<br/>
![IconSOText](https://github.com/DemonViglu/SchoolWork/blob/main/Image/TempFile/IconSOText.png)
- 如果希望中途将名字撤去，则在文本中独立一行打入`NULL`，则`DialogSystem`会关闭头像 _UI_ 和名字显示。<br/>
![IconSOText_Null](https://github.com/DemonViglu/SchoolWork/blob/main/Image/TempFile/IconSONULL.png)

- **注意：`IconSOManager`中的图片和名字要一一对应，否则将会播放错误**


## 多个DialogSystem之间的对话使用。

- 示例场景中有`DMM`的预制体。访问作者的GitHub仓库，到`Elysia`仓库中可以看到示例场景。
- 在两个`DialogSystem`对话前，需要向`DMM`发出请求，调用该脚本下的`AddRequest`函数。此处涉及到对话系统的ID，可以在`DialogSystemManager`脚本下的`DialogSystemID`进行配置。如果两个对话系统匹配成功，则一方可以向另一方对话系统申请调用它者下的一段对话`DialogMM:CallForOtherSystemSO`。具体暂时不细讲，此处功能虽然可以实现，但是尚未整理。

## DialogSystemManager的Player Setting参数介绍
1. _wordTimeGap_: 每一个字符输出的时间间隔(float)。
2. <a id="_sentenceTimeGap"> _sentenceTimeGap_ </a>: 每一句话输出的时间间隔(float)。（默认为一个回车是一句话，而非一个句号）。
1. _MissionTimeGap_： 每一个Mission之间的调用间隔(float)（该参数只在两个对话或两个对话树中间有效，可以让两个不同的对话有一些间隔，而一个对话树内部通过选项调用下一个对话，则不会有该时间参数间隔）。
2. _MissionEdgeTime_： 判断一个对话刚刚结束的结束状态保持时间(float)（与后面一个参数有关）.
3. _KeyToPassTheSentence_：(KeyCode)顾名思义，点击该Key的时候，该句子会直接输出完整，并进入sentenceTimeGap间隔，等待播放下一句话。
4. _JustFinishMission_: 该bool变量只是用于编辑器调试时方便观看，是私有变量，在上面变量设置的EdgeTime内，该变量为true。而在别的地方，可以调用函数 `DialogSystem.instance.JustFinishPlay()`函数来获取该变量。
5. _MissionList_: 私有变量，用于调试看当前有哪些对话任务待播放。（支持往里面疯狂塞对话，会形成队列逐个播放）。

## 一些其他的使用和函数
### DialogSystemManager 下的一些公开方法
- `AddMission(DialogMission mission)`<br/>
该方法可以在其他地方new 一个 C语言原生的**DialogMission**类，参数SO类型的Mission相同，会将该任务添加到对话待播放的列表尾部。
- `ClearMissionRightNow()`<br/>
清除当前的播放任务。
- `ClearAllMission()`<br/>
清除所有任务（包括当前任务和待执行任务）。
- `StartMissionSO(int index)`<br/>
清除所有任务，剩余作用于`AddMissionSO(int index)`相同。建议不要使用。
- `AddMissionSOAtFirst(int index)`<br/>
将该对话SO添加至播放列表的最前面。（是用于对话树插队的，使用者若有需要，如现在立马插入一个对话，则可以选择`ClearMissionRightRow`之后马上`AddMissionSOAtFirst`。PS:目前没有做插入暂存当前对话的功能）。
- `bool IsOnMission()`<br/>
是否在播放对话当中。
- `bool IsOutPutWord()`<br/>
是否在打字，与上一个函数不同，当Mission不再打字而是在sentenceTimeGap间隔中，仍算OnMission，但不算OutPutWord
- `bool IsOnMissionGap()`<br/>
是否在两个任务中间的MissionTimeGap中间等待。
- `JustFinishPlay()`<br/>
是否刚刚完成一个对话或对话树。与MissionEdgeTime一同使用。
- `GetDialogSystemID()`<br/>
获取该dialogSystem的id。
