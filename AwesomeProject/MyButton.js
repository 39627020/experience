/**
 * Sample React Native App
 * https://github.com/facebook/react-native
 * @flow
 */

import React, { Component } from 'react';
import {
  Platform,
  StyleSheet,
  Text,
  View,
  TouchableHighlight,
  ToastAndroid
} from 'react-native';
var num;
class MyButton extends Component {
  _onPressButton() {
    ToastAndroid.show("You tapped the button!" + num, 1000);
    console.log("You tapped the button!" + num);
  }

  render() {
    return (
        <TouchableHighlight onPress={this._onPressButton}>
          <Text>Button</Text>
        </TouchableHighlight>
    );
  }
}

export default class App extends Component<{}> {
  render() {
    num = parseInt(Math.random()*2);
    return (
        <View style={styles.container}>
          <MyButton />
        </View>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
});