//��������ͺ���������ؽ�������

#include <stdio.h>
#include <iostream>
#include <string>
#define MAXN 66000

using namespace std;

/*
* ���ȼ�ס��Ҫ����Ķ�����
* �ڵ�ṹ�塢Tree���顢node_numȫ�ֱ���
* create()��������������һ����㡢build()�����������ݹ齨��
* ���������������������б���
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
    return &Tree[node_num++]; //�ص�����1��������ȡ��ַ���Լ�
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
        ret->lchild = build(mid_s, root_pos -1, post_s, post_s + (root_pos -1) - mid_s); //�ص�����2��������ֱ�ӵ��ڵ�������ϣ��ڶ������һ�
    }
    if(root_pos!= mid_e)
    {
        ret->rchild = build(root_pos+1, mid_e, post_e - mid_e +root_pos, post_e-1); // �ص�����3��������ֱ�ӵ��ڵ������ȥ���ڶ������һ�
                                                                                    // �ڶ������һ��������еĳ��ȡ����⣬�����ǰ������򣬷�������
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

