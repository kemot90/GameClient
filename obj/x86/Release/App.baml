   M S B A M L   `   `   `  ����   	RPGClient    RPGClient.App   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       �� 	Resources     
noneBorder �� 
TargetType �� Triggers P� Property P� Value �� Property �� Value �� 
TargetType'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup	   Transitions%  System.Windows.VisualTransition
   GeneratedDuration   States   System.Windows.VisualState   
Storyboard �� TargetProperty �� 
TargetName8  2System.Windows.Media.Animation.EasingColorKeyFrame �� KeyTime �� Value ��Name �� RadiusY �� RadiusX �� EndPoint �� 
StartPoint � Color � Offset J� SnapsToDevicePixels �� Triggers �� 
TargetName �� CornerRadius ��Name��%o  &	        '�� �     '�� �     '�� �    �� " �� P�  �@J�IsMouseOver . . ���  � ��
 ���� ��  � ��$ 0q�P�  �@J�IsMouseOver .  . ���  � ��
 ���� ��  � ��$ 0q��� " ��.���  � ��
 ������� " ��.���  � ��
 �������  � �� �� " ��.R��    CommonStates��	   $
 0:0:0.3D�.   
Normal��  	MouseOver��. �� .���� $H B(Shape.Fill).(GradientBrush.GradientStops)[1].(GradientStop.Color)�$ 	rectangle��.g�  $ 0��$ 	#88391F00���� $H B(Shape.Fill).(GradientBrush.GradientStops)[0].(GradientStop.Color)�$ 	rectangle��.g�  $ 0��$ 	#DD000000���� $, &(Shape.Stroke).(SolidColorBrush.Color)�$ 	rectangle��.g�  $ 0��$ 
DarkOrange��  Pressed��  Disabled��.+��� $ 	rectangle��$ 5��$ 5��
���   �$��2������ $ 0.5,1&�$ 0.5,0&�.��� $ 	#DD000000��$ 0Q�� $ 	#88232323��$ 1Q��� #��z����. # z #��z�� P�  �@J�	IsFocused . P�  �@��IsDefaulted . P�  �@J�IsMouseOver . P�  � �� . P�  � {� .  . ���  � �� �	#FFBBBBBB�� $ 	rectangle�� � �� �	#FF999999�� " ��.���  � �� �� " ��.R�� .+��� $��Border��$��0,0,0,-2q�
������ 
���   �$��0q�$ 5 5 0 0��._��� $ ContentSite��$��Center=�$��Center=�$��Header��$��	12,8,12,2q���.  P�  �@��
IsSelected . . ���  �@K�ZIndex$	 100���� $ Border�� � �� �	#CC000000�� $ Border�� � ��$ 0q���  � �� �#FFFFFFP�  �@��
IsSelected .  . ��� $ Border�� � �� �	#00000000�� $ Border�� � ��
 ���� ��  � �� �#FFFFFF P�  �@��
IsSelected . . ���  � ��
 ���� ��  � ��$ 0q�P�  �@��
IsSelected .  . ���  � ��
 ���� ��  � ��$ 0q�