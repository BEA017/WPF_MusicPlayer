   M S B A M L   `   `   `  ????N  JWPF_MusicPlayer_ekz, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:WPF_MusicPlayer_ekzWPF_MusicPlayer_ekz  5   	   -    'WPF_MusicPlayer_ekz.Enter_playlist_name   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:WPF_MusicPlayer_ekz     5   	     ? Title$  Enter playlist name??5   	   $	??200??6%   $	??300??62   ???	#FF1D1D1D ? Icon$ 	/Logo.png??6V    ? 	Resources 5	         WatermarkTextBox ?? 
TargetType ?? Property ?? Value ?? 
TargetType ??Name ??Name	 ?? RelativeSource
 ?? Mode ?? Path ??Name ?? Triggers ]? 
Conditions ?? Property ?? Value ?? 
TargetName P? Property P? Value??%?  &	        ?? 5
   
   " ??6)   .?5      ??  ? ?? 5      ?? 5      " ??6&   .R?5      ? .+?5      ?? $ 
MainBorder??6%   ???	#FFBD6812$??1.4,1.4,1,1q?6P   
???????._?5   "   ?? $ 	TextLabel??6(   #??z??5   !   $y?Hidden=?6K   $|?False??5   !   
?????????5   !   ?? 	 ?? $
 TemplatedParent=?$	 Tag?5   3   5      ?? 5      $ PART_ContentHost??66   $??3q?6+   
???   ?6P   5       5      ]? 5       5   "   ?? 5   &    ?@J?	IsFocused$ False??6E   ?? 5   &    ? ??$  ??6@   ./?5   "   ?? $ 	TextLabel??6?    ? y?$ Visible=?6V   ?? 5   "   $ 	TextLabel??6?    ? ?? ?	#FFBD68126V   5      P? 5        ?@J?	IsFocused$
 True??6;   . ?5!   "   ?? $ 
MainBorder??6@    ? ??
 ?   ?6X   5"      P? 5#       ? {?$ False??6;   . ?5$   "   ??  ? ?? ?	#FF3333336?   5%      5'      5)      5*      5+      .??5,      ? .+?5-   
   ?? -   5         ??Name$ nameBox??5-      $??Center=?6#   $??0,46,0,0q?6@   $	??Top=?6U   $	??252??6m   $??31??6y   $??Arial)?6?   $??16(?6?    ? Cursor$ IBeam??6?   
 ? Tag$ Enter playlist name...??5.      #??[  69   ?? 5/   
   -   5        $
??Save??5/      $??Center=?6    $	??Top=?6=   ???	#FFFF7500$??33??6l   $	??124??6x   $??20(?6?   $??Arial Black)?6?   $??	0,106,0,0q?6?   51      52      