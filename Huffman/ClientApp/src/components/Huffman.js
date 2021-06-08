import React, { Component } from 'react';
import { TreevizReact } from 'treeviz-react';

export class Huffman extends Component {
    static displayName = Huffman.name;

    constructor(props) {
        super(props);
        this.state = { value: '', tree: null, loading: true, treeValue: null, myTreeData: null, avg: null, entropy: null };
    }

    handleChange = event => {    
        this.setState({value: event.target.value});  
    }

    componentDidMount() {
    }

    render() {

        return (
            <div>
                    <input type="text" value={this.value} onChange={this.handleChange} />
                    <input type={"submit"} value={"Generate"} onClick={() => this.GenerateTreeData(this.content)}/>
                <div>{JSON.stringify(this.state.tree)}</div>
                <div>Entropy: {this.state.entropy}</div>
                <div>AVG: {this.state.avg}</div>
                <TreevizReact 
                    data = {this.state.myTreeData}
                    idKey={'id'}
                    relationnalField={'father'}
                    nodeWidth={200}
                    nodeHeight={100}
                    mainAxisNodeSpacing={2}
                    secondaryAxisNodeSpacing={1.3}
                    renderNode={(node) =>
                        `<div style="height:${node.settings.nodeHeight}px;display:flex;align-items:center;margin-left:12px">Node value: ${node.data.text_1}, ${node.data.text_2 != null? `Node Content: ${node.data?.text_2}`: null}</div>`
                    }
                    onNodeClick={(node) => console.log('you clicked on node ' + node.id)}
                    duration={500}
                    linkWidth={(node) => 3}
                    />
            </div>
        );
    }
    
    async GenerateTreeData() {
        var response = await fetch(`huffman/jsonTree?content=`+ this.state.value, { 
            method: 'POST'}
        );
        var body = await response.json();
        console.log(body);
        this.setState({tree: body.tree, entropy: body.entropy, avg: body.avg});        
        
        console.log(body.tree)
        let dataToShow = this.GetDataFromNode(JSON.parse(body.tree), null, null)
        
        console.log(dataToShow)
        
        
        this.setState({myTreeData: dataToShow.resultFromNode})
    }

    GetDataFromNode(node, fatherId, usedIds){
        let resultFromNode = []
        if(usedIds == null)
            usedIds = 0;
        
        console.log(node.left)
        
        if(fatherId == null)
            resultFromNode.push({id: ++usedIds, text_1: node.Frequency, text_2: node.Symbol, father: null })
        else if(node.Left == null && node.Right == null)
            resultFromNode.push({id: ++usedIds, text_1: node.Frequency, text_2: node.Symbol, father: fatherId })
        else resultFromNode.push({id: ++usedIds, text_1:node.Frequency, father: fatherId});

        let ownId = usedIds
        
        
        if(node.Left != null) 
        {
            let nodeContent = this.GetDataFromNode(node.Left, ownId, usedIds)
            usedIds = nodeContent.usedIds;
            resultFromNode.push(...nodeContent.resultFromNode)
        }

        if(node.Right != null)
        {
            let nodeContent = this.GetDataFromNode(node.Right, ownId, usedIds)
            usedIds = nodeContent.usedIds;
            resultFromNode.push(...nodeContent.resultFromNode)
        }
        
        return {usedIds: usedIds, resultFromNode: resultFromNode}
    }
    
    async populateWeatherData() {
        const response = await fetch('weatherforecast');
        const data = await response.json();
        this.setState({ forecasts: data, loading: false });
    }
}
