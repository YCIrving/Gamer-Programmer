#include<iostream>
#include<string>
using namespace std;
/*˼·����
1.��ȡ����С��(��������û��'+'��)x��y��
2.��õ�һ�������෴����
3.���������淶������Ϊ�÷���λ��ʾ����ʽ(0.x,1.x)��
4.���������Ĳ���(����һ������ĺ���)��
5.��x��-x���˫����λ��ͬʱȥ��С���㣻
6.��yȥ��С���㲢�͸���λ����һ��������
7.����Booth�㷨���壬���岿�ֻ����䳤��Ϊx�ĳ��ȡ�����ѭ������counter��
8.����ӷ�����������Ҫ���Ƕ������⣻
9.������λ������
10.����󣬽����ת����ԭ������������ţ���
*/

/*��������
0.1101,0.1011
output=+0.10001111

-0.1011,-0.1101
output=+0.10001111

-0.11011011,-0.100101101
output=+0.10000000101111111

+0.11011011,-0.100101101
output=-0.10000000101111111

-0.1101,+0.1001
output=-0.01110101

+0.1001,-0.1101
output=-0.01110101
*/

/*�﷨�ص�
1.��������Ҫ�漰���������;����ַ��������Զ��ַ����Ĳ������﷨���ص㣻
2.string::find_last_of(char c)���ҵ�c���һ�γ��ֵ�λ�ã�
3.string::insert(int n,char* s)��ע��sΪ�ַ�������ԭ�ַ���[n]ǰ�����ַ���s��
4.�ַ���������switch�ж���
5.�����ַ����������л���һ���õ������ã�����ʱ����Ҫ��"&"��ֻ�ں�������ʱд�����ˡ�
*/

string opposite(string str)//���෴��
{
    if(str[0]=='-')
        str[0]='+';
    else if(str[0]=='+')
        str[0]='-';
    else if(str[0]=='0')
        str='-'+str;
    return str;
}
string normalize(string str)//�淶��
{
    if(str[0]=='-')
        str='1'+str.substr(2,str.length()-2);
    else if(str[0]=='+')
        str=str.substr(1,str.length()-1);
    return str;
}
string getComplement(string str)//����
{
    if(str[0]=='0')
        return str;
    int pos=0;
    pos=str.find_last_of('1');
    for(int i=2;i<pos;i++)
    {
        if(str[i]=='1')
            str[i]='0';
        else if(str[i]=='0')
            str[i]='1';
    }
    return str;
}
string extendSignBit(string x)//��չΪ˫����λ
{
    x[1]=x[0];
    return x;
}
string normalizeY(string y)//ȥ��������С���㣬��������λ
{
    y=y[0]+y.substr(2,y.length()-2)+'0';
    return y;
}
string getOriginal(string str)//��ԭ��
{
    if(str[0]=='+')
        return str;
    int pos=0;
    pos=str.find_last_of('1');
    for(int i=3;i<pos;i++)
    {
        if(str[i]=='1')
            str[i]='0';
        else if(str[i]=='0')
            str[i]='1';
    }
    return str;
}
string add(string s1,string s2)//�Ӻ���
{
    int carry=0;
    int sum=0;
    string output(s1.length(),'0');
    for(int i=s1.length()-1;i>=0;i--)
    {
        sum=carry+s1[i]-'0'+s2[i]-'0';
        if(sum==0)
        {
            carry=0;
            output[i]='0';
        }
        else if(sum==1)
        {
            carry=0;
            output[i]='1';
        }
        else if(sum==2)
        {
            carry=1;
            output[i]='0';
        }
        else if(sum==3)
        {
            carry=1;
            output[i]='1';
        }
    }
    return output;
}
void rightShift(string &output,string &y_com)//���ƺ���
{
    y_com=output.substr(output.length()-1,1)+y_com.substr(0,y_com.length()-1);
    output=output.substr(0,2)+output[1]+output.substr(2,output.length()-3);
}
void Booth(string x_com,string x_oppCom,string y_com)//Booth�㷨
{
    string output(x_com.length(),'0');//�����ʼ��
    string temp;
    int counter=y_com.length()-2;//����ѭ������
    for(int i=0;i<=counter;i++)
    {
        temp=y_com.substr(y_com.length()-2,2);
        if(temp=="10")
        {
            output=add(output,x_oppCom);
            if(i!=counter)
            rightShift(output,y_com);
        }
        else if(temp=="01")
        {
            output=add(output,x_com);
            if(i!=counter)
            rightShift(output,y_com);
        }
        else
        {
            if(i!=counter)
            rightShift(output,y_com);
        }
    }
    if(output[0]=='0')
        output[0]='+';
    else if(output[0]=='1')
        output[0]='-';
    output[1]='0';
    output.insert(2,".");
    output+=y_com.substr(0,counter);
    output=getOriginal(output);
    cout<<output;
}
int main()
{
    string x,x_opposite,y;
    string x_com,x_oppCom,y_com;
    string input;
    getline(cin,input);
    //cin>>input;
    int pos=0;
    pos=input.find(',');
    x=input.substr(0,pos);
    y=input.substr(pos+1,input.length()-pos-1);
    x_opposite=opposite(x);
    //�෴��
    x=normalize(x);
    y=normalize(y);
    x_opposite=normalize(x_opposite);
    //�õ��淶����������
    x_com=getComplement(x);
    x_oppCom=getComplement(x_opposite);
    y_com=getComplement(y);
    //�õ��������������������ʽ����
    x_com=extendSignBit(x_com);
    x_oppCom=extendSignBit(x_oppCom);
    y_com=normalizeY(y_com);
    //�õ����������������ֵ
    Booth(x_com,x_oppCom,y_com);
    return 0;
}
