   M S B A M L   `   `   `  ����   	RPGClient    RPGClient.Interface   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � 
ResizeMode$  NoResize=� � WindowStyle$
 None=� � AllowsTransparency .  � Title$ 	Interface��$	��900��$
��1366�� � WindowState$ 	Maximized=� � WindowStartupLocation$ CenterScreen=�����  j� Stretch$
 None=� j� Viewport$ 512,512,512,512� j� ViewportUnits$ Absolute=�	 j� TileMode$
	 Tile=�
 �� ImageSource$.
 (/RPGClient;component/Images/grunge02.jpg��.���  � RowDefinitions �� $	��125 ��� $��* �.+�� $��0�� � ColumnDefinitions �� $	��300 ��� $	��222 ��� $��* ��� $	��200 �.+��� $	��290��$	��115�����	#CC000000
���   �$��3q� �� CornerRadius$ 10�� �� Padding$ 10q�$��5q�$��0��$
��Left=�._��� .+�� .+��� -    ��Name$ characterNameTop��$��imie postaci��$��14(�$
��Bold"��� $��14(�$
��Bold"�$�� (Lvl. ���� -   $ characterLvlTop��$��14(�$
��Bold"��� $��14(�$
��Bold"�$��)��� .+��� $+��%/RPGClient;component/Images/coins.png��$��20��$��20��$��0,0,5,0q��� -   $ goldTop��$��14(�$
��Bold"�$��0���� $��14(�$
��Bold"�$�� szt.���� Co się dzieje z postacią:�� -   $ characterStatusInfo��$��Status���� $��1��$
��Left=�.+��� $	��218��$��40������ $3
 -/RPGClient;component/Images/statborder_hp.png��.+��� -    ��Name$ HPBar��$��22��$	��200�� �� Left$ 9��
 �� Top$ 9������ $ 
0,0,200,22�$ Absolute=�$
	 Tile=�$.
 (/RPGClient;component/Images/hpstatus.png���� $	��218�� �� Orientation$ 
Horizontal=�$ 12��.+��� -   $ HPStatus��$	��218�� �� TextAlignment$ Center=�
HP: 75/100�� $	��218��$��40������ $4
 ./RPGClient;component/Images/statborder_con.png��.+��� -   $ ConBar��$��22��$	��150��$ 9��$ 9������ $ 
0,0,200,22�$ Absolute=�$
	 Tile=�$5
 //RPGClient;component/Images/conditionstatus.png���� $	��218��$ 
Horizontal=�$ 12��.+��� -   $ ConditionStatus��$	��218��$ Center=�Kon: 75/100�� $	��318��$��40������ $/
 )/RPGClient;component/Images/expborder.png��.+��� -	   $ ExpBar��$��22��$	��150��$ 9��$ 9������ $ 
0,0,200,22�$ Absolute=�$
	 Tile=�$)
 #/RPGClient;component/Images/exp.png���� $	��318��$ 
Horizontal=�$ 12��.+��� -
   $ 	ExpStatus��$	��318��$ Center=�Exp: 50/100�� $��3��$
��Left=�.+��� -   $��logout��$��Wyloguj��$��23��$��75���� Zalogowany jako:�� -   $ idText��$��Identyfikacja��$
��Left=�$	��Top=�
��������� -   $ serverTimeStamp��$��	TimeStamp��$
��Left=�$	��Top=�
��������� ���	#CC000000$��1����Z� .@��� -   $��	postacTab��$��0�� �� 
IsSelected . ���� $ 
Horizontal=�.+��� $��50��$��50���� $��Center=�$��5,0,0,0q��� $�Postać��.���� $
��Left=�.+�� $��Center=�$��0,0,0,5q�.+��� $
��Left=�$��Center=�$��80��$��26,0,0,0q�
��� ���Cecha:�� $��Right=�$��Center=�$ Right=�$��50��
��� ���Baza:�� $��Right=�$��Center=�$ Right=�$��50��
��� ���Bonus:�� -   $ remainingPoints��$ Right=�$��80��
��� ���� $��Center=�$��0,0,0,5q�.+��� $)��#/RPGClient;component/Images/str.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Siła:�� -   $ characterStrength��$��Right=�$��Center=�$ Right=�$��50������  �� ElementName$ character.Strength���� -   $ characterStrengthBonus��$��Right=�$��Center=�$ Right=�$��50���� -   $ streangthSum��$ Right=�$��Center=�$��60���� -    ��Name$ incStr��$��20��$��20��
 � Tag$ strength��$��+��� $��Center=�$��0,0,0,5q�.+��� $*��$/RPGClient;component/Images/stam.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Wytrzymałość:�� -   $ characterStamina��$��Right=�$��Center=�$ Right=�$��50���� -   $ characterStaminaBonus��$��Right=�$��Center=�$ Right=�$��50���� -   $ 
staminaSum��$ Right=�$��Center=�$��60���� -   $ incStam��$��20��$��20��$ stamina��$��+��� $��Center=�$��0,0,0,5q�.+��� $)��#/RPGClient;component/Images/dex.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Zręczność:�� -   $ characterDexterity��$��Right=�$��Center=�$ Right=�$��50���� -   $ characterDexterityBonus��$��Right=�$��Center=�$ Right=�$��50���� -   $ dexteritySum��$ Right=�$��Center=�$��60���� -   $ incDex��$��20��$��20��$ 	dexterity��$��+��� $��Center=�$��0,0,0,5q�.+��� $*��$/RPGClient;component/Images/luck.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Szczęście:�� -   $ characterLuck��$��Right=�$��Center=�$ Right=�$��50���� -   $ characterLuckBonus��$��Right=�$��Center=�$ Right=�$��50���� -   $ luckSum��$ Right=�$��Center=�$��60���� -   $ incLuck��$��20��$��20��$
 luck��$��+���� -    $��
Dodaj expa��$��30���� -!   $��mapaTab��$��1��$��Stretch=�$��Stretch=����� $ 
Horizontal=�.+��� $-��'/RPGClient;component/Images/mapIcon.png��$��50��$��50���� $��Center=�$��5,0,0,0q��� $
�Mapa��.���� .���� -"    ��Name$ mapContainer��$	��873��.+��� $ 0��$ 0�� K� ZIndex$ 0��$)��#/RPGClient;component/Images/map.jpg��$
��1280��$	��873���� -#   $��equipmentTab��$��1������ $ 
Horizontal=�.+��� $/��)/RPGClient;component/Images/equipIcon.png��$��50��$��47���� $��Center=�$��5,0,0,0q��� $�	Ekwipunek��.���  �� $	��700 ��� $��* �.+��� $	��700��$��Hidden=�$��Hidden=�$��0��.��� $	��675��.+��� $ 12��$��5q�$ 5,5,5,5q����	#88000000���	#EE000000$��2q�._��� $
��Left=�$��0,0,0,5q�$	��200��.+��� $��Center=�$��Center=�
��� ���$��22��$��15(� � DataContext �� Broń:�� -$    ��Name$ weaponSpotImg��$3��-/RPGClient;component/Images/weaponEnabled.png��$��41��$��41��$��Stretch=�$��Stretch=�$��LeftToRight=�$��0��� .+��� $
��Left=�$��Center=�$��42��Nazwa:�� -%   $ 
weaponName��$��Right=�$��Center=�$ Right=�$	��126��� .+��� $
��Left=�$��Center=�$��80��Obrażenia:�� -&   $ weaponAttack��$��Right=�$��Center=�$ Right=�$��88��
���  ��$
��Bold"�� .+��� $)��#/RPGClient;component/Images/str.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Siła:�� -'   $ weaponStrength��$��Right=�$��Center=�$ Right=�$��66��$
��Bold"�
��� � �� .+��� $*��$/RPGClient;component/Images/stam.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Wytrzymałość:�� -(   $ weaponStamina��$��Right=�
��� � �$��Center=�$ Right=�$��66��$
��Bold"�� .+��� $)��#/RPGClient;component/Images/dex.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Zręczność:�� -)   $ weaponDexterity��$��Right=�
��� � �$��Center=�$ Right=�$��66��$
��Bold"�� .+��� $*��$/RPGClient;component/Images/luck.gif��$��26��$��25��$��0,0,0,0q��� $
��Left=�$��Center=�$��80��Szczęście:�� -*   $ 
weaponLuck��$
��Left=�
��� � �$��Center=�$ Right=�$��66���� -+   $ weaponTakeoff��$��Zdejmij��$��30��$	��100���� $ 12��$��5q�$ 5,5,5,5q����	#88000000���	#EE000000$��2q�._��� $
��Left=�$��0,0,0,5q�$	��200��.+��� $��Center=�$ Center=�$��Center=�
��� ���$��22��$��15(� �� Napierśnik:�� -,   $ chestSpotImg��$2��,/RPGClient;component/Images/chestEnabled.png��$��41��$��41��$��Stretch=�$��Stretch=�$��LeftToRight=�$��0��� .+��� $
��Left=�$��Center=�$��42��Nazwa:�� --   $ 	armorName��$��Right=�$��Center=�$ Right=�$	��126��� .+��� $
��Left=�$��Center=�$��80��Punkty zbroi:�� -.   $ 
armorArmor��$��Right=�$��Center=�$ Right=�$��88��
���  ��$
��Bold"�� .+��� $)��#/RPGClient;component/Images/str.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Siła:�� -/   $ armorStrength��$��Right=�$��Center=�$ Right=�$��66��$
��Bold"�
��� � �� .+��� $*��$/RPGClient;component/Images/stam.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Wytrzymałość:�� -0   $ armorStamina��$��Right=�
��� � �$��Center=�$ Right=�$��66��$
��Bold"�� .+��� $)��#/RPGClient;component/Images/dex.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Zręczność:�� -1   $ armorDexterity��$��Right=�
��� � �$��Center=�$ Right=�$��66��$
��Bold"�� .+��� $*��$/RPGClient;component/Images/luck.gif��$��26��$��25��$��0,0,0,0q��� $
��Left=�$��Center=�$��80��Szczęście:�� -2   $ 	armorLuck��$
��Left=�
��� � �$��Center=�$ Right=�$��66���� -3   $ chestTakeoff��$��Zdejmij��$��30��$	��100���� $ 12��$��5q�$ 5,5,5,5q����	#88000000���	#EE000000$��2q�._��� $
��Left=�$��0,0,0,5q�$	��200��.+��� $��Center=�$ Center=�$��Center=�
��� ���$��22��$��15(� �� Nagolenniki:�� -4   $ bootsSpotImg��$2��,/RPGClient;component/Images/bootsEnabled.png��$��41��$��41��$��Stretch=�$��Stretch=�$��LeftToRight=�$��0��� .+��� $
��Left=�$��Center=�$��42��Nazwa:�� -5   $ legsName��$��Right=�$��Center=�$ Right=�$	��126��� .+��� $
��Left=�$��Center=�$��80��Punkty zbroi:�� -6   $ 	legsArmor��$��Right=�$��Center=�$ Right=�$��88��
���  ��$
��Bold"�� .+��� $)��#/RPGClient;component/Images/str.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Siła:�� -7   $ legsStrength��$��Right=�$��Center=�$ Right=�$��66��$
��Bold"�
��� � �� .+��� $*��$/RPGClient;component/Images/stam.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Wytrzymałość:�� -8   $ legsStamina��$��Right=�
��� � �$��Center=�$ Right=�$��66��$
��Bold"�� .+��� $)��#/RPGClient;component/Images/dex.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Zręczność:�� -9   $ legsDexterity��$��Right=�
��� � �$��Center=�$ Right=�$��66��$
��Bold"�� .+��� $*��$/RPGClient;component/Images/luck.gif��$��26��$��25��$��0,0,0,0q��� $
��Left=�$��Center=�$��80��Szczęście:�� -:   $ legsLuck��$
��Left=�
��� � �$��Center=�$ Right=�$��66���� -;   $ bootsTakeoff��$��Zdejmij��$��30��$	��100���� $ 12��$��5q�$ 5,5,5,5q����	#88000000���	#EE000000$��2q�._��� $
��Left=�$��0,0,0,5q�$	��200��.+��� $��Center=�$ Center=�$��Center=�$��78��
��� ���$��22��$��15(� �� Hełm:�� -<   $ helmetSpotImg��$3��-/RPGClient;component/Images/helmetEnabled.png��$��41��$��41��$��Stretch=�$��Stretch=�$��LeftToRight=�$��0��� .+��� $
��Left=�$��Center=�$��42��Nazwa:�� -=   $ headName��$��Right=�$��Center=�$ Right=�$	��126��� .+��� $
��Left=�$��Center=�$��80��Punkty zbroi:�� ->   $ 	headArmor��$��Right=�$��Center=�$ Right=�$��88��
���  ��$
��Bold"�� .+��� $)��#/RPGClient;component/Images/str.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Siła:�� -?   $ headStrength��$��Right=�$��Center=�$ Right=�$��66��$
��Bold"�
��� � �� .+��� $*��$/RPGClient;component/Images/stam.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Wytrzymałość:�� -@   $ headStamina��$��Right=�
��� � �$��Center=�$ Right=�$��66��$
��Bold"�� .+��� $)��#/RPGClient;component/Images/dex.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Zręczność:�� -A   $ headDexterity��$��Right=�
��� � �$��Center=�$ Right=�$��66��$
��Bold"�� .+��� $*��$/RPGClient;component/Images/luck.gif��$��26��$��25��$��0,0,0,0q��� $
��Left=�$��Center=�$��80��Szczęście:�� -B   $ headLuck��$
��Left=�
��� � �$��Center=�$ Right=�$��66���� -C   $ helmetTakeoff��$��Zdejmij��$��30��$	��100���� $ 12��$��5q�$ 5,5,5,5q����	#88000000���	#EE000000$��2q�._��� $
��Left=�$��0,0,0,5q�$	��200��.+��� $��Center=�$ Center=�$��Center=�$��78��
��� ���$��22��$��15(� �� 	Tarcza:�� -D   $ shieldSpotImg��$3��-/RPGClient;component/Images/shieldEnabled.png��$��41��$��41��$��Stretch=�$��Stretch=�$��LeftToRight=�$��0��� .+��� $
��Left=�$��Center=�$��42��Nazwa:�� -E   $ 
shieldName��$��Right=�$��Center=�$ Right=�$	��126��� .+��� $
��Left=�$��Center=�$��80��Punkty zbroi:�� -F   $ shieldArmor��$��Right=�$��Center=�$ Right=�$��88��
���  ��$
��Bold"�� .+��� $)��#/RPGClient;component/Images/str.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Siła:�� -G   $ shieldStrength��$��Right=�$��Center=�$ Right=�$��66��$
��Bold"�
��� � �� .+��� $*��$/RPGClient;component/Images/stam.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Wytrzymałość:�� -H   $ shieldStamina��$��Right=�
��� � �$��Center=�$ Right=�$��66��$
��Bold"�� .+��� $)��#/RPGClient;component/Images/dex.gif��$��26��$��25���� $
��Left=�$��Center=�$��80��Zręczność:�� -I   $ shieldDexterity��$��Right=�
��� � �$��Center=�$ Right=�$��66��$
��Bold"�� .+��� $*��$/RPGClient;component/Images/luck.gif��$��26��$��25��$��0,0,0,0q��� $
��Left=�$��Center=�$��80��Szczęście:�� -J   $ 
shieldLuck��$
��Left=�
��� � �$��Center=�$ Right=�$��66���� -K   $ shieldTakeoff��$��Zdejmij��$��30��$	��100���� 
������ $��1����Z� .@��� $��0�� . ���� $ 
Horizontal=�.+��� $-��'/RPGClient;component/Images/weapons.png��$��30��$��30���� $��Center=�$��5,0,0,0q�Bronie.���� $	��450��$��Hidden=�$
��Left=�.���� -L    ��Name$ 
weaponList��.+��� Bronie�� $��1������ $ 
Horizontal=�.+��� $,��&/RPGClient;component/Images/armors.png��$��30��$��30���� $��Center=�$��5,0,0,0q�
Pancerze.���� $	��450��$��Hidden=�$
��Left=�.���� -M   $ 	armorList��.+��� 
Pancerze�� -N   $��fightTab��$��1������ $ 
Horizontal=�.+��� $5��//RPGClient;component/Images/warriorimagemi6.png��$��50��$��47���� $��Center=�$��5,0,0,0q��� $�Walka��.���� .+��� Walka: