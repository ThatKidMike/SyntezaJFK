namespace Synteza {

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    class Rewriter : CSharpSyntaxRewriter {


        public override SyntaxNode VisitBinaryExpression(BinaryExpressionSyntax node) {

            if (node.Kind() == SyntaxKind.EqualsExpression) {

                BinaryExpressionSyntax equalsExpression = node;
               
                if (equalsExpression.GetFirstToken().Kind() == SyntaxKind.IdentifierToken
                    && equalsExpression.GetLastToken().Kind() != SyntaxKind.IdentifierToken) {
                
                    ExpressionSyntax left = node.Left.WithoutTrailingTrivia();
                    ExpressionSyntax right = node.Right.WithTrailingTrivia(equalsExpression.GetFirstToken().TrailingTrivia);
                    SyntaxToken sign = node.OperatorToken;

                    node = node.ReplaceNode(node, SyntaxFactory.BinaryExpression
                        (SyntaxKind.EqualsExpression, right, sign, left));                

                }
                   
              
            }
            return base.VisitBinaryExpression(node);
        }

        




    }
}
