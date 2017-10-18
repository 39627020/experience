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
    Button,
    ToastAndroid
    } from 'react-native';
var num;
class Guess extends Component {
    _onPressButton() {
        if(this.props.value==num){
            ToastAndroid.show("You right!" + num, 1000);
        }else{
            ToastAndroid.show("Try again!", 1000);
        }
    }

    render() {
        return (
            <Button
                onPress={this._onPressButton}
                title={this.props.value}
                />
        );
    }
}

export default class App extends Component<{}> {
    render() {
        num = parseInt(Math.random()*2);
        return (
            <View style={styles.container}>
                <Guess value="0" />
                <Guess value="1" />
                <Guess value="2" />
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