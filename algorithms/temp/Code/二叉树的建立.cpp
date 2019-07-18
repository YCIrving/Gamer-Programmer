#include<iostream>
using namespace std;
class node
{
    public:
    int data;
    node *left,*right;
    node(int i,node* left,node *right)
    {
        this->data=i;
        this->left=left;
        this->right=right;
    }
};
void initTree(node **tree)
{
    int i;
    cin>>i;
    if(i==-1)
        *tree=NULL;
    else
    {
        *tree=new node(i,NULL,NULL);
        initTree(&(*tree)->left);
        initTree(&(*tree)->right);
    }
}
void preOrder(node *tree)
{
    if(tree==NULL)
    {
        return;
    }
    else
    {
        cout<<tree->data;
        preOrder(tree->left);
        preOrder(tree->right);
    }
}
int main()
{
    node *tree=new node(-1,NULL,NULL);
    initTree(&tree);//如果不传tree的地址，则不能更改实际tree的值
    preOrder(tree);

}
