namespace Synteza {

    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    class Rewriter : CSharpSyntaxRewriter {


        public override SyntaxNode VisitBinaryExpression(BinaryExpressionSyntax node) {

            if (node.Kind() == SyntaxKind.EqualsExpression) {

                bool wasVariableInside = false;

                ExpressionSyntax equalsExpressionRight = node.Right;

                IEnumerable<SyntaxNode> list = equalsExpressionRight.DescendantNodesAndSelf();

                foreach(SyntaxNode n in list) {

                    if(n is IdentifierNameSyntax) {

                        //Console.WriteLine(n.ToString());
                        wasVariableInside = true;

                    } 

                }          
                  
                if (!wasVariableInside) {
                
                    ExpressionSyntax left = node.Left.WithoutTrailingTrivia();
                    ExpressionSyntax right = node.Right.WithTrailingTrivia(node.GetFirstToken().TrailingTrivia);
                    SyntaxToken sign = node.OperatorToken;

                    node = node.ReplaceNode(node, SyntaxFactory.BinaryExpression
                        (SyntaxKind.EqualsExpression, right, sign, left));                

                }
                   
              
            } else if(node.Kind() == SyntaxKind.NotEqualsExpression)  {

                ExpressionSyntax notEqualsExpressionRight = node.Right;

                bool wasVariableInside = false;

                IEnumerable<SyntaxNode> list = notEqualsExpressionRight.DescendantNodesAndSelf();


                foreach (SyntaxNode n in list) {

                        if (n is IdentifierNameSyntax) {

                            //Console.WriteLine(n.ToString());
                            wasVariableInside = true;

                        }

                    }

                if (!wasVariableInside) {

                    ExpressionSyntax left = node.Left.WithoutTrailingTrivia();
                    ExpressionSyntax right = node.Right.WithTrailingTrivia(node.GetFirstToken().TrailingTrivia);
                    SyntaxToken sign = node.OperatorToken;

                    node = node.ReplaceNode(node, SyntaxFactory.BinaryExpression
                        (SyntaxKind.EqualsExpression, right, sign, left));

                }


            }

            return base.VisitBinaryExpression(node);
        }

        




    }
}
