struct Node
{
    Node * lchild;
    Node * rchild;
    int num;
}Tree[MAXN];

int node_num=0;

Node* create()
{
    Tree[node_num].lchild=NULL;
    rchild = NULL;
    return &Tree[node_num++]
}

Node * build(int s1, int e1, int s2, int e2)
{
    Node * root = create();
    root->num = post[e2];
    int pos;
    find post[e2] int mid;

    if(pos != s1)
    {
        root->lchild = build(s1, pos -1, s2, s2+pos-1-s1);
    }

    if(pos!= e1)
    {
        root->rchild = build(pos+1, e1, e2-e1+pos, e2-1);
    }
    return root;
}

void preOrder(Node* root)
{
    cout<<root->num;
    if(root->lchild!=NULL)
        preOrder(lchild);
    ...
}