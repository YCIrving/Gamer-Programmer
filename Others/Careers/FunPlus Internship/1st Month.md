# 第一月：

**时间**：2019.05.20-2019.06.20

**目标**：能够独立完成基本的问题解决和开发工作，融入团队

---

# Day1-2: 2019.05.20 - 2019.05.21

入职并配置环境

# Day 3: 2019.05.22

**今天主要的成绩是熟悉了开发环境，领到了任务，并且完成了提交。**

**同时希望仿照[_马里奥制造2_](https://www.youtube.com/watch?v=jPi-u0D8sQ4)，开发一个地图编辑器的拖拽放置功能。**

## 开发环境

开发过程中主要用到的软件包括：Unity、RIder、IntelliJ、Excel、Terminal、Jira、SourceTree

* Unity：预览各类游戏场景和进行试玩
* Rider：编写和调试Unity工程中涉及的C_#_代码
* IntelliJ：编写和调试Unity工程中涉及的Lua代码
* Excel：查看游戏策划维护的各种表格
* Terminal：运行脚本，用于将excel表格转化为json和lua等游戏中可以被读取的格式
* Jira：在线协作平台，用于领取和完成任务
* SourceTree：拉取和推送代码

## Terminal转化表格

在`wod-shared`目录下运行`./conv.sh all`命令即可

## IntelliJ与Unity的协作

1. 搜索并安装_AmmyLua_插件，可以让Lua代码高亮显示

   ![IntelliJ Plugins](assets\IntelliJ Plugins.png)

2. 在IntelliJ中选择从源码新建工程，然后根目录选择Unity工程中的Lua目录

   ![IntelliJ New Project](assets\IntelliJ New Project.png)

## Rider 与Unity的协作

1. 如果安装了Rider，可以直接在Unity中双击C_#_文件，即可自动打开Rider

2. 在Rider中，可以通过输出或者加入断点的方式来进行代码调试，输出除了使用标准的`print()`之外，还可以使用定义好的`Debug.Log(Message)`方法来输出Message信息。如果是添加断点，在添加之后，需要开启调试模式，即右上角的绿色Bug，之后在Unity中运行游戏，即可在断点中停止。

   ![Rider Debug](assets\Rider Debug.png)

## 提交任务

1. 今天还改了一个Bug，是关于地图中建筑物种类的，建筑物共有21种，种类编号为1到21，而数组从0-20，所以编号作为数组下标时需要进行`-1`操作，而代码漏掉了`-1`，导致删除出错。

2. 修改代码并确认没问题后就可以进行提交了，提交时注意在暂存区只保留自己修改的代码，并且在右侧确认所有修改部分都是正确的，都确认好后，在commit中输入注释，提交即可。

3. 今天在拉取时出现了另一种冲突，即本地文件比远端文件多的情况，在SourceTree中以蓝色形式展现，不同于黄色的相异，这种冲突不能通过重置解决，解决方式为删除本地的文件，选择移除即可。

   ![SourceTree Conflict](assets\SourceTree Conflict.png)



   