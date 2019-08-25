#include <iostream>
#include <vector>
#include <algorithm>
#include <math.h>

#define MAX 60000
using namespace std;
struct Node
{
    Node* parent;
    Node* lchild;
    Node* rchild;
    int content;
}Tree[MAX];
int cnt=0;
vector<int> inorder, postorder, preorder;
Node* create()
{
    Tree[cnt].lchild=Tree[cnt].rchild=Tree[cnt].parent=NULL;
    return &Tree[cnt++];
}
Node* buildTree(int s1, int e1, int s2, int e2)
{
    Node* root=create();
    root->content=postorder[e2];
    int j;

    int pos=0;
    for(int i=s1;i<=e1;i++)
    {
        if(inorder[i]==postorder[e2])
        {
            pos=i;
            break;
        }
    }
    if(pos!=s1)
    {
        root->lchild=buildTree(s1,pos-1,s2,s2+pos-s1-1);
    }
    if(pos!=e1)
    {
        root->rchild=buildTree(pos+1,e1,s2+pos-s1,e2-1);
    }
    return root;
}
void preOrder(Node* tree)
{
    cout<<tree->content<<' ';
    if(tree->lchild!=NULL)
    {
        preOrder(tree->lchild);
    }
    if(tree->rchild!=NULL)
    {
        preOrder(tree->rchild);
    }
}
int main()
{
    Node* tree;
    string s;
    int num=0,cnt=0,nodeNum=0;
    getline(cin,s);
    reverse(s.begin(),s.end());
    for(int i=0;i<s.length();i++)
    {
        if(s[i]!=' '&& s[i]!='\0')
        {
            num+=(s[i]-'0')*pow(10,cnt);
            cnt++;
        }
        else
        {
            inorder.push_back(num);
            num=0;
            cnt=0;
        }
    }
    inorder.push_back(num);
    reverse(inorder.begin(),inorder.end());

    nodeNum = inorder.size();
    for(int i=0;i<nodeNum;i++)
    {
        cin>>num;
        postorder.push_back(num);
    }
//    for(int i=0;i<nodeNum;i++)
//    {
//        cout<<inorder[i]<<' ';
//    }
//    cout<<endl;
//    for(int i=0;i<nodeNum;i++)
//    {
//        cout<<postorder[i]<<' ';
//    }
//    cout<<endl;

    tree=buildTree(0,nodeNum-1,0,nodeNum-1);
    preOrder(tree);

    return 0;
}
