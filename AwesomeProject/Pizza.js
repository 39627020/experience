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
    TextInput,
    View,
    Image
    } from 'react-native';

class PizzaTranslator extends Component {
    constructor(props) {
        super(props);
        this.state = {text: ''};
    }

    render() {
        return (
            <View style={{padding: 10}}>
                <TextInput
                    style={{height: 40,width:200}}
                    placeholder="Type here to translate!"
                    onChangeText={(text) => this.setState({text})}
                    />
                <Text style={{padding: 10, fontSize: 42}}>
                    {this.state.text.split(' ').map((word) => word && '🍕').join(' ')}
                </Text>
            </View>
        );
    }
}

export default class App extends Component<{}> {
    render() {
        return (
            <View style={{flex: 1, flexDirection: 'column',justifyContent: 'center',alignItems: 'center',}}>
                <PizzaTranslator />
            </View>
        );
    }
}