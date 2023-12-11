# DialogSystem(Unity)
 一个简小但是足够满足一定基础的对话系统。

## 初始使用介绍
### 初始设置步骤：
1. 将package文件夹中的DialogSystem2添加到项目之中。
![](https://img-blog.csdnimg.cn/direct/3713d1c9fc2c4b12a0b42f9e930e1023.png#pic_center)
2. 在Hierarchy窗口面板中创建一个空物体(建议命名成DialogSystem)，添加代码**DialogSystemManager**和**DialogMissionEventHandler**，并将**DialogMissionEventHandler**拖拽到**DialogSystemManager**的相应位置。
3. 在Hierarchy窗口面板中创建一个Canvas,一个带有子物体Text的Panel，两个button(为了区分，下文分别叫做button_1和button_2)，并将两个button和panel设为**不可见**。（如图两个button分别叫Option_1和Option_2）。
4. 将刚刚创建的text，panel,两个button拖拽到**DialogSystemManager**上面，并且button_1的onClick处调用**DialogSystemManager**的
```C#
PlayMissionOption_1();
```
  button_2的onClick处调用**DialogSystemManager**的
```C#
PlayMissionOption_2();
```
<br/>![](https://img-blog.csdnimg.cn/direct/4ec1e963ea2944a3b5aa9c0cfe03a947.png?#pic_center)

5. 在Asset文件夹下创建一个DialogMissionSO文件夹，并右键点击"Create"->"DialogMission"->"New DialogMissionSOManager",并将该SO文件拖拽到DialogSystemManager的相应位置。
![](https://img-blog.csdnimg.cn/direct/78f763c1e3e84415851c0fcc8c7c04f3.png#pic_center)


### 创建第一个DialogMissionSO:
1. 在刚刚创建的DialogMissionSO文件夹下，右键点击"Create"->"DialogMission"->"New DialogMissionSO"。该SO共有五个变量，分别为 _Text Str_ , _Text Asset_ , _Option Mission Index_ , _Option Mission Description_。 在默认情况下，**DialogSystemManager** 会优先读取 _Text Str_ 中的字符作为对话文本， 其次才是文件格式的 _Text Asset_ 。<br/>
   对话文本Example：<br/>
   Hello<br/>
   World<br/>
则，对话会分两次输出，第一次输出 _Hello_ , 第二次输出 _World_。
2. 将该SO文件拖拽至**DialogMissionSOManager**的list下，并记好该文件在list中的index。（由于是第一个SO），则默认为0。
![](https://img-blog.csdnimg.cn/direct/ad2aa73bae95474cb94d015e2542979b.png#pic_center)


### 播放刚刚创建的第一个DialogMissionSO:
1. 为了方便，可以在Canvas组件下再创建一个button，并将其命名为 _AddMissionSO_ ， 并在其onclick处调用**DialogSystemManager**下的`AddMissionSO()`函数，并将参数赋值为刚刚记录的0。
2. 点击play按钮开始运行当前场景，点击 _AddMissionSO_ 按钮，会发现panel组件自动亮起，并且播放了刚刚的对话。（但是此时对话是一闪而过，这涉及变量修改，会在后文介绍），但至此，对话的基本功能已经实现完毕。

## 如何实现连续的对话树功能
### DialogMissionSO 两个List变量介绍:
- Option Mission Index<br/>
   记录该Mission之后会有的Mission下标（在DialogMissionSOManager下的下标）。由于**DialogSystemManager**的代码只写了支持两个button的函数（可以使用者自行仿照补充，或者我无聊了更新一下），所以这里最多记录两个下标。例如，在第一个创建的 _DialogMissionSO_ 的OptionMissionIndex列表下添加1和2，则表明该对话会有两个选项，分别每一个选项分别对应DialogMissionSOManager中列表1的对话SO和2的对话SO。
- Option Description <br/>
   记录该Mission的对话选项会是什么文字内容
### 创建新的两个DialogMissionSO:
- 仿照上文的第一个创建方式,分别命名为“Mission_2”和“Mission_3”,并添加到**DialogMissionSOManager**下。
- 将创建的第一个DialogMissionSo的OptionMissionIndex列表下添加1和2，并且添加两个Option Description，分别为“Mission_2”和“Mission_3”。
### 播放对话树：
- 点击play按钮开始运行当前场景，点击 _AddMissionSO_ 按钮，会发现第一段对话一闪而过，并且场景中点亮了两个按钮，上面的文字分别为“Mission_2”和“Mission_3”。至此，我们已经尝试了作为对话树的所有基本功能。<br/>
![](https://img-blog.csdnimg.cn/direct/e6ee1426d2fd47f4b5d105ed25d83c30.png#pic_center)
如图为当前版本包的demo表现形式。


## 如何实现在触发选项的同时触发相应的事件：
### DialogMissionSO 的 eventIndex介绍：
1. 该默认值应为-1，表示触发该对话的时候没有任何事件发生。如果其不为-1，则表明该对话任务在**DialogMissionEventHandler**有相应的**DialogMissionEvent**需要调用，并且会调用下标为eventIndex的**DialogMissionEvent**。
### 创建DialogMissionEvent：
1. 推荐在Hierarchy窗口面板下创建一个空物体，并命名为"MissionEvent_1",并添加**DialogMissionEvent**组件。
2. 将要在该对话任务调用的函数加入到该组件下的 _Option Event_中。<br/>
![](https://img-blog.csdnimg.cn/direct/f753391bbc6340aa8e3c64c69a6866bb.png#pic_center)

4. 将该物体“MissionEvent_1”拖拽至物体DialogManager的**DialogMissionEventHandler**下。由于是第一个任务，所以应当其下标为**0**。
5. 若设置在播放Mission_3的时候调用该模块事件，只需要找到相应的SO，并将其EventIndex修改为该下标0。
### 实现该模块：
- 点击play按钮开始运行当前场景，点击 _AddMissionSO_ 按钮，会发现第一段对话一闪而过，并且场景中点亮了两个按钮，上面的文字分别为“Mission_2”和“Mission_3”,点击Mission_3，会发现即可调用刚刚所设置的函数。

## DialogSystemManager的Player Setting参数介绍
1. _wordTimeGap_: 每一个字符输出的时间间隔(float)。
2. _sentenceTimeGap_: 每一句话输出的时间间隔(float)。（默认为一个回车是一句话，而非一个句号）。
3. _MissionTimeGap_： 每一个Mission之间的调用间隔(float)（该参数只在两个对话或两个对话树中间有效，可以让两个不同的对话有一些间隔，而一个对话树内部通过选项调用下一个对话，则不会有该时间参数间隔）。
4. _MissionEdgeTime_： 判断一个对话刚刚结束的结束状态保持时间(float)（与后面一个参数有关）.
5. _KeyToPassTheSentence_：(KeyCode)顾名思义，点击该Key的时候，该句子会直接输出完整，并进入sentenceTimeGap间隔，等待播放下一句话。
6. _JustFinishMission_: 该bool变量只是用于编辑器调试时方便观看，是私有变量，在上面变量设置的EdgeTime内，该变量为true。而在别的地方，可以调用函数 `DialogSystem.instance.JustFinishPlay()`函数来获取该变量。
7. _MissionList_: 私有变量，用于调试看当前有哪些对话任务待播放。（支持往里面疯狂塞对话，会形成队列逐个播放）。

## 一些其他的使用和函数
### 动态切换说话者的图片
-Image：该Header下共有三个变量，分别对应npc1的图片，npc2的图片和图片应该出现的地方。在代码**DialogSystemManager**下的函数`SetTextUI()`可以看到，如果当前句子为一个npc的名字，便会切换faceImage成相应npc的图片。要想拓展或修改该功能，目前应当去代码处手动添加修改。
### DialogSystemManager 下的一些公开方法
- `AddMission(DialogMission mission)`<br/>
该方法可以在其他地方new 一个 C语言原生的**DialogMission**类，参数SO类型的Mission相同，会将该任务添加到对话待播放的列表尾部。
- `ClearMissionRightNow()`<br/>
清除当前的播放任务。
- `ClearAllMission()`<br/>
清除所有任务（包括当前任务和待执行任务）。
- `StartMissionSO(int index)`<br/>
清除所有任务，剩余作用于`AddMissionSO(int index)`相同.
- `AddMissionSOAtFirst(int index)`<br/>
将该对话SO添加至播放列表的最前面。（是用于对话树插队的）。
- `bool IsOnMission()`<br/>
是否在播放对话当中。
- `bool IsOutPutWord()`<br/>
是否在打字，与上一个函数不同，当Mission不再打字而是在sentenceTimeGap间隔中，仍算OnMission，但不算OutPutWord
- `bool IsOnMissionGap()`<br/>
是否在两个任务中间的MissionTimeGap中间等待。
- `JustFinishPlay()`<br/>
是否刚刚完成一个对话或对话树。与MissionEdgeTime一同使用。
