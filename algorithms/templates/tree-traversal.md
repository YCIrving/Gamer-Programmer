# 二叉树的遍历

二叉树一般有两种遍历方式，DFS和BFS，其中DFS又分为后序(Postorder)、前序(Preorder)、中序(Inorder)，具体看下图：

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

- 后序遍历

    具体参考[LeetCode145](../LeetCode/problems/145.binary-tree-postorder-traversal.md)

    - 后序遍历（递归）:
        ```c++
        // recursive postorder binary tree traversal
        void postorderTraversalRecursive(TreeNode* root)
        {
            if (node == NULL)
                return;

            // first recur on left subtree
            postorderTraversalRecursive(node->left);

            // then recur on right subtree
            postorderTraversalRecursive(node->right);

            // now deal with the node
            cout << root->val << " ";
        }
        ```


    - 后序遍历（非递归）:
    ```c++
        vector<int> postorderTraversal(TreeNode *root) {
            vector<int> ret;
            if(!root) return ret;
            stack<TreeNode*> st;
            st.push(root);
            st.push(root);
            TreeNode *cur;
            while(!st.empty()){
                cur = st.top();
                st.pop();
                if(!st.empty()&&st.top() == cur){
                    if(cur->right) {
                        st.push(cur->right);
                        st.push(cur->right);
                    }
                    if(cur->left){
                        st.push(cur->left);
                        st.push(cur->left);
                    }
                }
                else
                    ret.push_back(cur->val);
            }
            return ret;
        }
    ```

    - 后序遍历还存在一种伪后序，思想是将前序遍历（根左右）稍作修改为“根右左”，然后将遍历结果逆序输出即可。这里采用实现简单的双栈法：

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

