/*
�������Ŀǰ�������������ӵ�һ�������Ѷȵ������ˡ�
˼·�ܼ򵥣�������ת��Ϊʮ����Ȼ�����õ�ȡ�����������Ӧ���Ƶ����֡�
����֮��о�������Ż��Ŀռ仹�Ǻܴ�ġ�
���⣬һλͬѧ�Ĵ����ҷ���ע�����ˣ��㷨Ӧ��û�����⣬���ǲ�����33��)��������C++�ڴ���
����ʱ�ĸ�����ʽ����ת��Ū��/(��o��)/~~
*/
#include<iostream>
#include<string>
#include<math.h>
using namespace std;
int main()
{
    string input,mid;
    long int temp=0;
    double a;
    int n,m;//�������n���ƣ����m����
    char output[100]={'\0'};
    int counter=0;
    cin>>n>>input>>m;
    for(int i=input.length()-1;i>=0;i--)
    {
        switch (input[i])
        {
            case 'A':
            a=10;
            break;
            case 'B':
            a=11;
            break;
            case 'C':
            a=12;
            break;
            case 'D':
            a=13;
            break;
            case 'E':
            a=14;
            break;
            case 'F':
            a=15;
            break;
            default:
                a=input[i]-'0';
        };
        temp+=a*pow(n,input.length()-1-i);
    }

   while(temp>m)
   {
       switch(temp%m)
       {
        case 10:
        output[counter]='A';
        break;
        case 11:
        output[counter]='B';
        break;
        case 12:
        output[counter]='C';
        break;
        case 13:
        output[counter]='D';
        break;
        case 14:
       output[counter]='E';
        break;
        case 15:
        output[counter]='F';
        break;
        default:
        output[counter]=temp%m+'0';
       }
       temp/=m;
       counter++;
   }
   switch(temp)
       {
       case 10:
        output[counter]='A';
        break;
        case 11:
        output[counter]='B';
        break;
        case 12:
        output[counter]='C';
        break;
        case 13:
        output[counter]='D';
        break;
        case 14:
        output[counter]='E';
        break;
        case 15:
        output[counter]='F';
        break;
        default:
        output[counter]=temp+'0';
       };
       for(int i=counter;i>=0;i--)
       {
           cout<<output[i];
       }
    return 0;
}
/*leimingmark 33��
# include<iostream>
# include<string.h>
# include<math.h>
using namespace std;
#define N 1000000
void iniToeve(int initial, int tra)//���㷨û����
{
	if (initial / tra)
		iniToeve(initial / tra, tra);
	if (initial % tra == 10)
		cout << 'A';
	else if (initial % tra == 11)
		cout << 'B';
	else if (initial % tra == 12)
		cout << 'C';
	else if (initial % tra == 13)
		cout << 'D';
	else if (initial % tra == 14)
		cout << 'E';
	else if (initial % tra == 15)
		cout << 'F';
	else
		cout << initial % tra;
}
int main()
{
	int n;//��������Ľ�����
	cin >> n;
	char cha[N];//����������ַ���ʽ�洢��
	cin >> cha;
	int ch_length = strlen(cha);//�ַ�����ĳ���
	int temp = ch_length - 1;
	int sum = 0;//���������ʮ���ƴ�С
	for (int i = 0; i < ch_length; i++)
	{
		switch (cha[i])//����Ӧ��������
		{
		case '0':
			sum = sum + 0 * pow(n, temp);
			break;
		case '1':
			sum = sum + 1 * pow(n, temp);
			break;
		case '2':
			sum = sum + 2 * pow(n, temp);
			break;
		case '3':
			sum = sum + 3 * pow(n, temp);
			break;
		case '4':
			sum = sum + 4 * pow(n, temp);
			break;
		case '5':
			sum = sum + 5 * pow(n, temp);
			break;
		case '6':
			sum = sum + 6 * pow(n, temp);
			break;
		case '7':
			sum = sum + 7 * pow(n, temp);
			break;
		case '8':
			sum = sum + 8 * pow(n, temp);
			break;
		case '9':
			sum = sum + 9 * pow(n, temp);
			break;
		case 'A':
			sum = sum + 10 * pow(n, temp);
			break;
		case 'B':
			sum = sum + 11 * pow(n, temp);
			break;
		case 'C':
			sum = sum + 12 * pow(n, temp);
			break;
		case 'D':
			sum = sum + 13 * pow(n, temp);
			break;
		case 'E':
			sum = sum + 14 * pow(n, temp);
			break;
		case 'F':
			sum = sum + 15 * pow(n, temp);
			break;
		default:
			break;
		}
		temp--;
	}
	int m;
	cin >> m;
	iniToeve(sum, m);
	return 0;
}

������ Result
    ���Ե� #1��ͨ���ò��Ե㡣 �÷�16����ʱ0ms���ڴ�3092kB��
    ���Ե� #2������Ĵ𰸡� �÷�0����ʱ0ms���ڴ�3088kB��
������ȷ�𰸳��ȣ�10 ��Ĵ𰸳��ȣ�10
�������������Ե������ 0% �ط���ʼ����ġ�
��һ�������ڵ� 10 ���ַ���ʼ���׼�����ͬ�ġ�
    ���Ե� #3������Ĵ𰸡� �÷�0����ʱ0ms���ڴ�3088kB��
������ȷ�𰸳��ȣ�30 ��Ĵ𰸳��ȣ�30
�������������Ե������ 0% �ط���ʼ����ġ�
��һ�������ڵ� 29 ���ַ���ʼ���׼�����ͬ�ġ�
    ���Ե� #4������Ĵ𰸡� �÷�0����ʱ15ms���ڴ�3096kB��
������ȷ�𰸳��ȣ�8 ��Ĵ𰸳��ȣ�8
�������������Ե������ 0% �ط���ʼ����ġ�
��һ�������ڵ� 6 ���ַ���ʼ���׼�����ͬ�ġ�
    ���Ե� #5������Ĵ𰸡� �÷�0����ʱ0ms���ڴ�3092kB��
������ȷ�𰸳��ȣ�13 ��Ĵ𰸳��ȣ�13
�������������Ե������ 0% �ط���ʼ����ġ�
��һ�������ڵ� 13 ���ַ���ʼ���׼�����ͬ�ġ�
    ���Ե� #6��ͨ���ò��Ե㡣 �÷�17����ʱ15ms���ڴ�3092kB��
*/
