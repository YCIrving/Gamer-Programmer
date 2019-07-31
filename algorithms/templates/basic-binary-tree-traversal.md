# 二叉树的基础遍历

二叉树的基础遍历一般有两种遍历方式，DFS和BFS，其中DFS又分为后序(Postorder)、前序(Preorder)、中序(Inorder)，具体看下图：

![img](assets/tree-traversal.png)

我们按照Leetcode的标准，定义一下树的结构：
```c++
/**
 * Definition for a binary tree node.
 * struct TreeNode {
 *     int val;
 *     TreeNode *left;
 *     TreeNode *right;
 *     TreeNode(int x) : val(x), left(NULL), right(NULL) {}
 * };
 */
```

下面对各种遍历方法进行总结：

## 后序遍历

具体参考[LeetCode145](../LeetCode/problems/145.binary-tree-postorder-traversal.md)

- 后序遍历（递归）:
    ```c++
    // recursive postorder binary tree traversal
    class Solution {
    private:
        // *M* 这里定义遍历函数，注意加引用，引用符号紧加在变量前
        void postorderTraversalResursive(TreeNode* root, vector<int> &ret)
        {
            if(!root) return;
            postorderTraversalResursive(root->left, ret);
            postorderTraversalResursive(root->right, ret);
            ret.push_back(root->val);
        }
    public:    
        // *M* leetcode默认的函数用来作为遍历函数的入口，并负责返回结果
        vector<int> postorderTraversal(TreeNode* root) {
            // *M* 在这里定义结果vector
            vector<int> ret;
            postorderTraversalResursive(root, ret);
            return ret;
        }
    };
    ```


- 后序遍历（非递归）:
    ```c++
    class Solution {
    public:
        vector<int> postorderTraversal(TreeNode* root) {
            vector<int> ret;
            stack<TreeNode*> stk;
            TreeNode* cur= root;
            while(1)
            {
                while(cur)
                {
                    // *M* 通过重复push来区分未遍历过右子树的节点
                    stk.push(cur);
                    stk.push(cur);
                    cur = cur->left;
                }
                if(!cur && stk.empty()) return ret;
                // *M* 因为之后还要判断，所以这里申请temp接收栈顶元素，不同于先序和中序，是直接使用cur接收
                TreeNode* temp = stk.top();
                stk.pop();
                // *M* 本质上就是上面代码的变种，只不过省去了判断右节点和交换节点的操作
                if(!stk.empty() && stk.top() == temp)
                {
                    cur = temp->right;
                }
                else
                {
                    ret.push_back(temp->val);
                }
            }
        }
    };
    ```

- 后序遍历（伪后序）：后序遍历还存在一种伪后序，思想是将前序遍历（根左右）稍作修改为“根右左”，然后将遍历结果逆序输出即可。这里采用实现简单的双栈法：

    ```c++
    class Solution {
    public:
        vector<int> postorderTraversal(TreeNode* root) {
            // *M* 需要特判
            if(!root) return {};
            stack<TreeNode*> stk1;
            stack<int> stk2;
            TreeNode* cur = root;
            stk1.push(cur);
            while(!stk1.empty())
            {
                // *M* 严格按照算法执行即可
                TreeNode* temp = stk1.top();
                stk1.pop();
                stk2.push(temp->val);
                if(temp->left) stk1.push(temp->left);
                if(temp->right) stk1.push(temp->right);
            }
            vector<int> ret;
            // *M* 对栈的不断弹出
            // 不要使用 for(int i=0; i<stk.size(); i++)
            // 因为栈的体积在不断变化，导致出错
            while(!stk2.empty())
            {
                ret.push_back(stk2.top());
                stk2.pop();
            }
            return ret;
        }
    };
    ```

## 先序遍历

具体参考[LeetCode144](../LeetCode/problems/144.binary-tree-preorder-traversal.md)

- 先序遍历（递归）:
    ```c++
    class Solution {
    public:
        void preorderTraversalRecursive(TreeNode* root, vector<int> &ret)
        {
            if(!root) return;
            ret.push_back(root->val);
            preorderTraversalRecursive(root->left, ret);
            preorderTraversalRecursive(root->right, ret);
        }

        vector<int> preorderTraversal(TreeNode* root) {
            vector<int> ret;
            preorderTraversalRecursive(root, ret);
            return ret;
        }
    };
    ```

- 先序遍历（非递归）:
    ```c++
    class Solution {
    public:
        vector<int> preorderTraversal(TreeNode* root) {
            vector<int> ret;
            stack<TreeNode*> stk;
            TreeNode* cur=root;
            while(1)
            {
                while(cur != NULL)
                {
                    // *M* 在这里输出，并将右节点压入栈中
                    ret.push_back(cur->val);
                    if(cur->right)
                    {
                        stk.push(cur->right);
                    }
                    cur=cur->left;
                }
                if(cur==NULL && stk.empty()) return ret;
                // *M* 只需要给cur赋值即可，并不需要后续的判断
                cur = stk.top();
                stk.pop();
            }
        }
    };
    ```

## 中序遍历
具体参考[LeetCode94](../LeetCode/problems/94.binary-tree-inorder-traversal.md)

- 中序遍历（递归）:
    ```c++
    class Solution {
    public:
        void inorderTraversalRecursive(TreeNode* root, vector<int> &ret)
        {
            if(!root) return;
            inorderTraversalRecursive(root->left, ret);
            ret.push_back(root->val);
            inorderTraversalRecursive(root->right, ret);
        }
        vector<int> inorderTraversal(TreeNode* root) {
            vector<int> ret;
            inorderTraversalRecursive(root, ret);
            return ret;
        }
    };
    ```

- 中序遍历（非递归）:
    ```c++
    class Solution {
    public:
        vector<int> inorderTraversal(TreeNode* root) {
            vector<int> ret;
            stack<TreeNode*> stk;
            TreeNode* cur=root;
            while(1)
            {
                while(cur!=NULL)
                {
                    // *M* 第一个与先序不同的点，这里不输出，而在下面输出
                    // *M* 第二个与先序不同的点，压入当前节点，而不是它的右节点                
                    stk.push(cur);
                    cur=cur->left;
                }
                if(cur==NULL && stk.empty()) return ret;
                cur = stk.top();
                stk.pop();
                ret.push_back(cur->val);
                // *M* 这里需要对cur进行一次操作
                cur=cur->right;
            }
        }
    };
    ```

## 层次遍历

- 简单层次遍历：不需要对数的层级进行区分，只需要按照层次将节点逐一输出即可，这样的遍历使用一个队列就可以完成。

    ```c++
    class Solution {
    public:
        vector<int> levelOrder(TreeNode* root) {
            // 以队列为主体，而不是cur，所以需要判空，代码结构也跟上面的DFS有所不同
            if(!root) return {};
            vector<int> ret;
            queue<TreeNode*> que;
            TreeNode* cur;
            que.push(root);
            while(!que.empty())
            {
                cur = que.front();
                que.pop();
                ret.push_back(cur->val);
                // *M* 这里要判空
                if(cur->left) que.push(cur->left);
                if(cur->right) que.push(cur->right);
            }
        }
    };
    ```

- 分层层次遍历：
具体参考[LeetCode102](../LeetCode/problems/102.binary-tree-level-order-traversal.md)
