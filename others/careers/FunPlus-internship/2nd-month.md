# 第二月：

**时间：2019.06.20-2019.07.20**

**目标：系统学习Unity和面向对象，准备秋招**



# Day 25: 06.27

由于组会需要讲论文以及有点感冒，鸽了两天公司的活。

先记录三个小发现：

- 熟悉C#中的`"\""`，表示只有一个引号的字符串;
- 发送给服务器解析命令行时，可以在有空格的数据比如userName、fileName等，前后加上引号， 这样就可以避免数据被空格打断;
- SourceTree 中仅修改文件名大小写不会算在修改文件里，所以对于一个文件，如果本地仅修改了文件名的大小写，是无法修改远端的。但如果在git config中将”ignorecase“设置为”false“，又会出现多push的情况，即远端会出现修改前和修改后的两个文件，这时需要将本地的文件备份，然后push一次，这样远端会认为本地将文件进行了删除，就会将大小写文件一并删掉。之后再将本地文件恢复，远端会认为新增了文件，而且新增的文件是正确的文件名。所以只需要再push一次即可。

# Day 26: 06.28
今天接到了新的需求，要求使用Unity中的IMGUI的Treeview做prefab的解析和与lua的绑定。基本上和培养要求中说的一致。

这个做一周，希望能把解析做好，之后继续做游戏中的背包系统和大地图城建，涉及wrpc的通信，即客户端与服务器之间的消息传递，听了一下leader的讲解，大致分为两种情况，一是客户端到服务器(C->S)，需要客户端发起请求，然后接受服务器传回的消息，主要通过lua中的回调来实现，大致是：
```
S:FunctionName(arg1, arg2, function (ret)
    // do somethind in invoke function
    end
)
```
其中的`function`是回调函数，可以嵌套；返回值可以多个，也可以是复杂的结构体。

另一种是服务器到客户端(S->C)，是服务器发起的请求，比如在客户端播放一个特效，客户端接收到请求，只需要做对应的响应即可。这种情况需要客户端实现服务器中同名的接口，每个函数有个ID，对应起来即可。这种函数返回值为void。

# Day 27-30: 06.29 - 07.04
这几天没有更新，有几点原因：

1. 需求有点复杂，而且得到的指导不是很够，所以自己探索起来动力不是很充足，而且一周之后需要换需求；
2. 实验室工作进度缓慢，论文也没有中，所以需要做一些实验室的工作；
3. 秋招的压力，需要刷题和刷面经，来准备即将到来的秋招；
4. 爱情方面也略有一些不愉快， 不过已经都解决好啦。

然后总结一下最近学到的东西：

1. SourceTree 时间长需要清理一下，只保留自己修改的文件
2. Unity中，一定要确保屏幕中要显示的内容每次都在OnGUI被刷新，否则就会发生点击后没反应的情况，从而误以为是逻辑问题，实际上是没有被显示出来。
3. Unity中的TreeView中默认的排序功能只能对顶层的节点进行排序，下层的子节点不参与排序。所以如果树的顶层节点只有一个，则排序功能就失效了。

# Day 31-32: 07.05, 07.07
最近主要完成了TreeView的展示功能，包括读取prefab的树形结构，读取其中的条目，按照树形结构显示在window中。但关于TreeView，自己理解的还不够，所以仅记录一些自己认为比较有用的吧。

1. 首先是逻辑部分，官网上封装的层级比较多，每个window包含一个treeview，负责显示树中的内容，每颗tree由不同的element组成，每个element包含深度、名称和id，存储为一个线性结构，按照深度和id恢复到原来的树形结构。
2. 添加节点:
```c#
void SetMvvmTreeElements (GameObject openedPrefab)
{
    _mvvmPrefab
    ScriptableObject.CreateInstance<MvvmPrefab>();
    var gameObjectTreeElements = new List<MvvmGameObject>();
    IDCounter = 0;
            
    var root = new MvvmGameObject("Root", -1, IDCounter);
    gameObjectTreeElements.Add(root);

    // 加入根节点后，排序功能会失效
    root = new MvvmGameObject(_openedPrefab, 0, ++IDCounter);
    gameObjectTreeElements.Add(root);

    foreach (Transform child in _openedPrefab.transform)
    {
            AddChildrenRecursive(root, child, gameObjectTreeElements);
    }
    _mvvmPrefab.treeElements = gameObjectTreeElements;
}

void AddChildrenRecursive(TreeElement parentElement, Transform child, List<MvvmGameObject> gameObjectTreeElements)
{

    var childAdded = new MvvmGameObject(child.gameObject, parentElement.depth + 1, ++IDCounter);
    gameObjectTreeElements.Add(childAdded);

    foreach (Transform grandChild in child.transform)
    {
            AddChildrenRecursive(childAdded, grandChild, gameObjectTreeElements);
    }
}
```
插入节点是一个BFS递归的过程，主要就是新建节点，并且初始化，然后加入到list中即可。

下周开始要实现背包系统和大地图城建了，感觉会更简单一些，同时也要开始正式准备秋招了。

# Day 33: 07.08
今天正式结束了Mvvm的部分，开始做背包系统的绑定，涉及lua和wrpc。记录一下昨天遇到的一个问题：
- 在IntelliJ中打开文件夹后，并不会自动导入文件夹中子文件夹的内容，此时还需要[设置一下根目录](https://www.jetbrains.com/help/idea/content-roots.html)：
打开文件->找到project structure->然后找到Modules->点击add content root，将根目录添加进去即可。

# Day 34: 07.12
1. 今天学习了一下界面绑定时，如何在Unity中更新美术资源：

    - 首先更新表格，刷新wrpc和wdsync
    - Unity中，点击Window->Asset Management->Addressable Assets，打开Addressables窗口，并将其移动到Scene和Game标签里
    - 把美术新增的prefab拖动到UIPrefabs中，并点击简化名称
    - 如果美术新增了贴图，在同目录下新建一个`Sprite Atlas`，与贴图文件夹同名，然后将贴图文件夹拖动到这个Atlas下，点击一下pack preview，最后将atlas拖动到UISprite中，简化名称，保存。
    ![Img](assets/unity-create-prefab&atlas.png)

2. 如果Unity在Hierarchy中修改了prefab，则需要在Inspector中选择Overrides -> Apply all，这里需要注意，如果修改的是子节点，则不存在Overrides按钮，此时需要回到父节点，然后才能找到Overrides。(也可以直接从Hierarchy中将prefab拖回到Project中)
    ![img](assets/prefab-overrides.png)


3. 在Console中，点击Collapse，可以将相同的错误折叠，方便观察其他错误。

# Day 35: 07.13
1. 查看Prefab在场景中的显示，可以将其拖动只Hierachy层级下的UIRoot中
2. 修改Unity中grid的元素排列方式，如果需要左对齐，要将选项里的"Cell Width Force Expand In Group"取消掉。

    ![img](assets/grid-cell-alignment.png)

3. SourceTree中拉取前要确认当前分支是否正确，即加粗的分支是当前分支

    ![img](assets/SourceTree-right-branch.png)

4. Intellij IDEA 中调试Lua代码

第二次调试会崩掉

# Day :
1. IntelliJ中，使用Shift+Delete，可以删除一整行。
