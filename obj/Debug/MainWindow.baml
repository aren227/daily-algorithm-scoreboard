   M S B A M L   `   `   `  ����L  HDailyAlgorithmWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:DailyAlgorithmWPFDailyAlgorithmWPF  5   	   "    DailyAlgorithmWPF.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:DailyAlgorithmWPF     5   	   -   5          � Title$  
MainWindow��5   	   $	��500��6   $	��300��6)   ���	#40000000 � AllowsTransparency .  � WindowStyle$
 None=�6f   .��5	      �� .+�5   
   �� -   5         ��Name$ ndays��5      $'��!알고리즘 폐관수련 n일차��6   $��0q�67   $��2q�6B   $	��Top=�6N   
�������$��14(�6y   $��메이플스토리)�6�   $��Center=�6�   �� 5   
    �� Orientation$ 
Horizontal=�6   $��
0, 5, 0, 0q�6.   .+�5      �� .+�5      �� -   5        $ total_solved��5      $��2q�6.   $��Total Solved��6:   ���#69a7ff$��14(�6f   $��메이플스토리)�6t   $��Center=�6�   $	��150��6�   �� 5      -   5        $ total_solved_count��5      $��0q�64   $
��1234��6@   ���#69a7ff$��40(�6d   $��메이플스토리)�6r   $��Center=�6�   $	��150��6�   5      �� 5      .+�5      �� -   5        $ daily_solved��5      $��2q�6.   $��Daily Solved��6:   ���#69a7ff$��14(�6f   $��메이플스토리)�6t   $��Center=�6�   $	��150��6�   �� 5      -   5        $ daily_solved_count��5      $��0q�64   $
��1234��6@   ���#69a7ff$��40(�6d   $��메이플스토리)�6r   $��Center=�6�   $	��150��6�   5      5      �� 5   
   -   5        $ content��5      $��활동 기록��6!   $��0, 10, 0, 5q�61   $��2q�6F   $	��Top=�6R   
�������$��14(�6}   $��메이플스토리)�6�   $��Center=�6�   �� 5   
   -   5         ��Name$
 feed��5      5      5      5      