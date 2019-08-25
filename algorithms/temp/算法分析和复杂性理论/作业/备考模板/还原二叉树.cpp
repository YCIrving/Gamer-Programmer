//根据中序和后序遍历，重建二叉树

#include <stdio.h>
#include <iostream>
#include <string>
#define MAXN 66000

using namespace std;

/*
* 首先记住需要定义的东西：
* 节点结构体、Tree数组、node_num全局变量
* create()函数：用来新增一个结点、build()函数，用来递归建树
* 遍历函数，用来对树进行遍历
*/
struct Node
{
    Node * lchild;
    Node * rchild;
    int num;
}Tree[MAXN];

int node_num;

Node * create()
{
    Tree[node_num].lchild = Tree[node_num].rchild = NULL;
    return &Tree[node_num++]; //重点记忆点1，别忘了取地址和自加
}

int midOrder[MAXN], postOrder[MAXN];

void preOrder(Node *root)
{
    cout<<root->num<<' ';
    if(root->lchild != NULL)
    {
        preOrder(root->lchild);
    }
    if(root->rchild != NULL)
    {
        preOrder(root->rchild);
    }
}

Node* build(int mid_s, int mid_e, int post_s, int post_e)
{
//    cout<<mid_s<<mid_e<<post_s<<post_e<<endl;
    Node* ret = create();
    ret -> num = postOrder[post_e];
    int root_pos;
    for(int i= mid_s; i<=mid_e; i++)
    {
        if(midOrder[i] == postOrder[post_e])
        {
            root_pos = i;
            break;
        }
    }
    if(root_pos!= mid_s)
    {
        ret->lchild = build(mid_s, root_pos -1, post_s, post_s + (root_pos -1) - mid_s); //重点记忆点2：第四项直接等于第三项加上（第二项减第一项）
    }
    if(root_pos!= mid_e)
    {
        ret->rchild = build(root_pos+1, mid_e, post_e - mid_e +root_pos, post_e-1); // 重点记忆点3：第三项直接等于第四项减去（第二项减第一项）
                                                                                    // 第二项减第一项就是序列的长度。另外，如果是前序加中序，方法类似
    }
    return ret;
}

int main()
{
    string str1, str2;
    getline(cin, str1);
    getline(cin, str2);
    node_num =0;
    int n=0;
    int cnt =0;
    for(int i=0; i<str1.length(); i++)
    {
        if(str1[i] == ' ')
        {
            midOrder[cnt++] = n;
            n=0;
        }
        else
        {
            n= n*10 + str1[i] - '0';
        }
    }
    midOrder[cnt++] = n;
    n=0;
    cnt = 0;
    for(int i=0; i<str2.length(); i++)
    {
        if(str2[i] == ' ')
        {
            postOrder[cnt++] = n;
            n=0;
        }
        else
        {
            n= n*10 + str2[i] - '0';
        }
    }
    postOrder[cnt++] = n;
    Node * T =build(0, cnt-1, 0, cnt-1);
    preOrder(T);
    cout<<endl;
}

