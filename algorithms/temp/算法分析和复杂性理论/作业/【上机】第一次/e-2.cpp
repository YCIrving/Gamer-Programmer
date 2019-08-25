#include <iostream>
#include <iostream>
#include <vector>
#include <algorithm>
#include <math.h>
using namespace std;
vector<int> inorder, postorder, preorder;
int preOrder(int s1, int e1, int s2, int e2)
{
    cout<<postorder[e2]<<' ';
    int rootPos;
    for(rootPos = s1; rootPos<=e1; rootPos++)
    {
        if(inorder[rootPos]==postorder[e2])
            break;
    }
    if(rootPos!=s1)//说明没有左子树
    {
        preOrder(s1,rootPos-1,s2,s2+rootPos-1-s1);
    }
    if(rootPos!=e1)//说明没有右子树
    {
        preOrder(rootPos+1,e1,e2-e1+rootPos,e2-1);
    }
    return 0;
}
int main()
{
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

    preOrder(0,nodeNum-1,0,nodeNum-1);
    //preOrder(tree);

    return 0;
}
