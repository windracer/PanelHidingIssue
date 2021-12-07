# Panel Hiding Issue
Full source code for the StackOverflow question

---
Description

I have a Grid with three columns - left, right, and a grid splitter in between. I need to hide one of the panels. What I need is to set Width=0 for two Splitter columns, and for the Panel column. However, this approach works well only from code. When I use [styles][1] or [value converters][2], it works only in some cases. 

Everything works as expeted until I move the splitter, panels hide but leave white space. Only the "code behind" way works correctly in all cases. The GIF image below illustrates the behavior.

[![Example of the wrong behavior][3]][3]

The code is shown below. The main idea is to set Width and MaxWidth properties for grid colums to 0, and then back to * for the panel, and to Auto for Splitter.

**1. The way it works (code behind):**
```lang-cs
private void TogglePanelVisibility(bool isVisible)
{
    if (isVisible)
    {
        // Restore saved parameters:
        _mainWindow.ColumnPanel.Width = new GridLength(_columnPanelWidth, GridUnitType.Star);
        _mainWindow.ColumnPanel.MaxWidth = double.PositiveInfinity;

        _mainWindow.ColumnSplitter.Width = new GridLength(_columnSplitterWidth, GridUnitType.Auto);
        _mainWindow.ColumnSplitter.MaxWidth = double.PositiveInfinity;
        return;
    }

    // Save parameters:
    _columnSplitterWidth = _mainWindow.ColumnSplitter.Width.Value;
    _columnPanelWidth = _mainWindow.ColumnPanel.Width.Value;

    // Hide panel:
    _mainWindow.ColumnPanel.Width = new GridLength(0);
    _mainWindow.ColumnPanel.MaxWidth = 0;
    _mainWindow.ColumnSplitter.Width = new GridLength(0);
    _mainWindow.ColumnSplitter.MaxWidth = 0;
}
```

**2. The way it doesn't work (XAML Style)**
```lang-xml
 <Window.Resources>
     <Style x:Key="showColumnStar" TargetType="{x:Type ColumnDefinition}">
         <Style.Setters>
             <Setter Property="Width" Value="*" />
             <Setter Property="MaxWidth" Value="{x:Static system:Double.PositiveInfinity}" />
         </Style.Setters>
         <Style.Triggers>
             <DataTrigger Binding="{Binding IsPanelVisible}" Value="False">
                 <DataTrigger.Setters>
                     <Setter Property="Width" Value="0" />
                     <Setter Property="MaxWidth" Value="0" />
                 </DataTrigger.Setters>
             </DataTrigger>
         </Style.Triggers>
     </Style>

     <Style x:Key="showColumnAuto" TargetType="{x:Type ColumnDefinition}">
         <Style.Setters>
             <Setter Property="Width" Value="Auto" />
             <Setter Property="MaxWidth" Value="{x:Static system:Double.PositiveInfinity}" />
         </Style.Setters>
         <Style.Triggers>
             <DataTrigger Binding="{Binding IsPanelVisible}" Value="False">
                 <DataTrigger.Setters>
                     <Setter Property="Width" Value="0" />
                     <Setter Property="MaxWidth" Value="0" />
                 </DataTrigger.Setters>
             </DataTrigger>
         </Style.Triggers>
     </Style>
 </Window.Resources>

 <!-- ... -->
 
 <Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="*" />
        <ColumnDefinition Width="Auto" Style="{StaticResource showColumnAuto}" />
        <ColumnDefinition Width="*" Style="{StaticResource showColumnStar}" />
    </Grid.ColumnDefinitions>
	<!-- ... -->
</Grid>
```

**3. And the last scenario using value converters**

XAML:
```lang-xml
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="*" />
        <ColumnDefinition Width="{Binding IsPanelVisible, Converter={StaticResource BoolToGridSizeConverter}, ConverterParameter='Auto'}" />
        <ColumnDefinition Width="{Binding IsPanelVisible, Converter={StaticResource BoolToGridSizeConverter}, ConverterParameter='*'}" />
    </Grid.ColumnDefinitions>
	<!-- ... -->
</Grid>
```

C#:
```lang-cs
internal class BoolToGridRowColumnSizeConverter : IValueConverter
 {
     public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
     {
         var param = parameter as string;
         var unitType = GridUnitType.Star;

         if (param != null && string.Compare(param, "Auto", StringComparison.InvariantCultureIgnoreCase) == 0)
         {
             unitType = GridUnitType.Auto;
         }

         return ((bool)value == true) ? new GridLength(1, unitType) : new GridLength(0);
     }

     public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
     {
         throw new NotImplementedException();
     }
 }
```
There is one more thing I learning from debugging the case #3 - on an attempt to show or hide the panel after moving the splitter, the Converted is calling only once. 

Could you please tell me what is happening, why the cases #2 and #3 do not work properly?

Thank you in advance!

  [1]: https://stackoverflow.com/a/51967350/44217
  [2]: https://stackoverflow.com/a/21884516/44217
  [3]: https://i.stack.imgur.com/3Hqq3.gif
