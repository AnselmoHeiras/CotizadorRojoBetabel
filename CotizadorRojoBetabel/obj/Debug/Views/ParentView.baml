   M S B A M L   `   `   `  ����O  KCotizadorRojoBetabel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:CotizadorRojoBetabel.ViewsCotizadorRojoBetabel.Views  5   	   +    %CotizadorRojoBetabel.Views.ParentView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   H DMahApps.Metro, Version=1.6.5.1, Culture=neutral, PublicKeyToken=null;controls,http://metro.mahapps.com/winfx/xaml/controls  5   	   4local(clr-namespace:CotizadorRojoBetabel.Views  5   	     � Title$  Cotizador Rojo Betabel��5	   	   $	��600��6(   $	��800��65    � Icon$8 2/CotizadorRojoBetabel;component/Resources/Icon.png��5
   	   ��5      �� 5   
    �� ImageSource$> 8/CotizadorRojoBetabel;component/Resources/Background.png��6   .��5      � ��5   
   �� 5       �� Color$ White��6    �� Opacity$	 0.3Q�6,    � ColumnDefinitions 5   
   �� 5      6!   �� 5      $��4* �6   6,   .+�5   
   � $��0��6    � RowDefinitions 5      �� 5      $
��1.5* �6    60   �� 5      6"   �� 5      6"   �� 5      6"   �� 5      6"   �� 5      6"   �� 5      6"   .+�5"      �� -   5         ��Name$ LogoImg��5"      $8��2/CotizadorRojoBetabel;component/Resources/Logo.png��6%   $��0��6a   �� 5$      -   5        $��ProductCatalogBtn��5$      $��	Productos��6.   $
��5,15q�6B   $��1��6P      RBRoundCornerButton#���   6]   $��16(�6�   
�������{�.   J� Effect 5%      3  -System.Windows.Media.Effects.DropShadowEffect  5&      	   Opacity$		 0.4Q�6'   5(      �� 5*      -   5        $��DishesCatalogBtn��5*      $��	Platillos��6-   $
��5,15q�6A   $��2��6O   #���   6\   $��16(�6�   
�������{�.   5+        5,      $		 0.4Q�6'   5.      �� 50      -   5        $��ConfigurationBtn��50      $��Configuración��6-   $
��5,15q�6E   $��6��6S   #���   6`   $��16(�6�   
�������{�.   51        52      $		 0.4Q�6'   54      56      8  2MahApps.Metro.Controls.TransitioningContentControl  58   
   -   5        
  Name$
 
Transition��59         
Transition$
 Down=�5:      $��1��5;      5=      5>      