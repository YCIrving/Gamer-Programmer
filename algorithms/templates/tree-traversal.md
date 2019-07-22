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

    练习可见[LeetCode145]((https://leetcode.com/problems/binary-tree-postorder-traversal/))


- 后序遍历（非递归）:


- 后序遍历还存在一种伪后序，思想是将前序遍历（根左右）稍作修改为“根右左”，然后将遍历结果逆序输出即可。
