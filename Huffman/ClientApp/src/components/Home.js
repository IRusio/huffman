import React, { Component } from 'react';
import {Huffman} from "./Huffman";

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>Hello, world!</h1>
          <Huffman/>
      </div>
    );
  }
}
