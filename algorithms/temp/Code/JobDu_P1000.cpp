/*
题目简述：A+B，但是注意这种判断输入是否结束的方法，C和C++都要会
*/

#include <iostream>
using namespace std;
int main()
{
    int a,b;
    while(std::cin >> a)
    {
        cin>>b;
        cout<<a+b<<endl;
    }
    return 0;
}
/*
#include <stdio.h>
int main()
{
  int a,b;
  while(scanf("%d%d",&a,&b)!=EOF)
    printf("%d\n",a+b );
  return 0;
}
