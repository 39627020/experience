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
    TouchableHighlight,
    ToastAndroid
    } from 'react-native';
var num;
function gennum(){
    num = parseInt(Math.random()*2);
}

class Guess extends Component {
    _onPressButton() {
        ToastAndroid.show(this.props.value,1000);
        if(this.props.value==num){
            ToastAndroid.show("You right!" + num, 1000);
        }else{
            ToastAndroid.show("Try again!", 1000);
        }
    }

    render() {
        return (
            <Button
                onPress={this._onPressButton.bind(this)}
                title={this.props.value}
                />
        );
    }
}

export default class App extends Component<{}> {
    render() {
        gennum();
        return (
            <View style={styles.container}>
                <View style={styles.row1}>
                    <TouchableHighlight
                        onPress={gennum()}>
                        <Text title='Try Again'></Text>
                    </TouchableHighlight>
                </View>
                <View style={styles.row}>
                    <Guess value="0" style={styles.item} />
                    <Guess value="1" style={styles.item} />
                    <Guess value="2" style={styles.item} />
                </View>
                <View style={styles.row}>
                    <Guess value="3" style={styles.item} />
                    <Guess value="4" style={styles.item} />
                    <Guess value="5" style={styles.item} />
                </View>
            </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        flexDirection : 'column',
        justifyContent: 'center',
        alignItems: 'center',
    },
    row1: {
      flex: 1,
      justifyContent: 'center',
      alignItems: 'stretch'
    },
    row: {
        flex: 1,
        flexDirection : 'row',
        justifyContent: 'space-between',
        alignItems: 'center',
    },
    item: {
        marginTop : 10,
        width: 20,
        height: 10
    }
});