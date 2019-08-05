# 刷题过程中的各种心得、技巧、规律和坑

### 做任何事都一样，在刚开始做时，不要追求极致和完美。写代码也一样，不要总是奢望找到问题的最优解或者写出最简洁的代码，现阶段能理解和记忆的才是最适合的。先做到，再做好。

## 链表
- 链表中的大部分问题只要细心，都能做出来
- 对于链表中一个节点的操作，它的前驱结点比它本身更加重要，因为根据它的前驱结点可以得到它本身以及后面的节点，而根据它本身只能得到它后面的节点。所以在遍历时，记得在合适的地方停下来。
- dummyHead用来处理对链表头部的修改，主要原因是链表头部没有前驱结点，造成了它与链表内部节点的差异。添加dummyHead可以消除这种差异。

## vector
- 可以用`vector.back()`来获得最后一个元素，而可以不必用`vector[vector.size()-1]`

## 二叉树
- 二叉树问题往往使用递归就能解决
- 二叉树遍历时，如果用到了栈，则有两种情况，一种是以栈为基准，不断从栈中提取元素进行处理，则需要首先把根节点加入栈中，则循环条件一般为`while(!stk.empty())`，并且需要在程序一开始进行空树特判`if(!root) return`。而如果是定义了一个指向节点的指针cur，并以cur为基准，栈只是临时保存cur相关的一些节点，则一般不需要特判，也不需要先将root加入到栈中，循环条件一般为`while(1)`里面有一个`while(cur)`和`if(!cur && stk.empty())`，第一个用来进行DSF遍历，第二个用来判断是否终止遍历。
- 上述问题中，两个循环的嵌套再加上对cur的判断是遍历的常见套路。为了理解这两点，多敲敲[leetcode145](LeetCode/problems/145.binary-tree-postorder-traversal.md)中的代码即可。

## 栈
- 对于栈的遍历，往往伴随着栈中元素的弹出，此时不能用
    ```c++
    for(int i=0; i<stk.size(); i++)
    {
        cout<<stk.top();
        stk.pop();
    }
    ```
    因为随着栈弹出，其size也在不断变化，导致循环次数并不能达到期望的次数。

    正确的做法是：
    ```c++
    while(!stk.empty())
    {
        cout<<stk.top();
        stk.pop();
    }
    ```

## 循环
- 写循环时遇到的一个问题，就是在循环体中修改循环变量，这时要用while而不是for，比如在一个字符串压缩的问题，要在一个字符串中找到一个字符连续出现的次数([报数问题](LeetCode/problems/38.count-and-say.md))
```c++
string stringParser(string s)
{
    string ret;
    int num;
    char c;
    int i=0, j;
    // *M* 对于修改循环变量的循环，用while而不要用for
    while(i<s.length())
    {
        c = s[i];
        num = 1;
        for(j = i+1; j<s.length(); j++)
        {
            if(s[j] == c) num++;
            else break;
        }
        // *M* 循环体内修改循环变量
        i=j;
        // *M* 这里不能用append()，因为append中的参数应该是字符串类型，而不是char
        ret.push_back('0'+num);
        ret.push_back(c);
    }
    return ret;
}
```

